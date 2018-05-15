using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Jober.Areas.API.Models;
using Jober.Data;
using Jober.Models;
using Jober.Models.UserViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Jober.Services.Informer
{
    public enum UserInformerOption { All, New, Work, Settings };

    public class UserInformer
    {
        private ApplicationDbContext Context { get; }

        public string UserId { get; }

        public UserInformer(ApplicationDbContext context, string userId)
        {
            Context = context;
            UserId = userId;
        }

        public UserInformer(ClaimsPrincipal user, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            Context = context;
            UserId = userManager.GetUserId(user);
        }

        public ApplicationUser GetUser()
        {
            return Context.Users.Where(u => u.Id == UserId).Include(u => u.City).SingleOrDefault();
        }

        public async Task<ApplicationUser> GetDataAsync()
        {
            return await Context.Users.Where(u => u.Id == UserId).Include(u => u.City).SingleOrDefaultAsync();
        }

        public async Task<ApplicationUser> GetUserWorkerAsync()
        {
            ApplicationUser userData = await GetDataAsync();
            if (userData == null)
                return null;

            userData.Worker = await Context.Workers.Where(w => w.UserId == this.UserId).SingleOrDefaultAsync();
            if (userData.Worker == null || !userData.Worker.IsActive)
                return null;

            return userData;
        }

        public async Task<UserPageViewModel> GetUserPageDataAsync()
        {
            ApplicationUser userData = await GetDataAsync();
            if (userData == null)
                return null;

            var userActiveOrders =
                await Context.Orders
                .Where(o => o.UserId == UserId 
                && (o.StatusId == (int)OrderStatus.Active || o.StatusId == (int)OrderStatus.InProgress))
                .Include(o => o.OrderDetails)
                .Include(o => o.Location)
                .ThenInclude(l => l.City)
                .ThenInclude(l => l.Districts)
                .OrderByDescending(o => o.Id).ToListAsync();

            if (userActiveOrders.Count > 0)
            {
                Context.Services.Load();
            }

            UserPageViewModel model = new UserPageViewModel
            {
                Name = userData.UserName,
                Email = userData.Email,
                IsEmailConfirmed = userData.EmailConfirmed,
                IsWorker = await Context.Workers.Where(w => w.UserId == UserId).Select(w => w.IsActive).SingleOrDefaultAsync(),
                PhoneNumber = userData.PhoneNumber,
                UserOrders = userActiveOrders,
                City = userData.City.Name,
                CityId = userData.CityId,
                Cities = await Context.Cities.ToListAsync(),
                Balance = userData.Balance
            };

            return model;
        }

        public async Task<WorkerPageViewModel> GetWorkerPageDataAsync(UserInformerOption dataOption)
        {
            ApplicationUser userWorker = await GetUserWorkerAsync();
            if (userWorker == null)
            {
                return null;
            }

            WorkerSetting workerSetting = await GetWorkerSetting(userWorker);

            List<Order> newOrders = null;
            if (dataOption == UserInformerOption.All || dataOption == UserInformerOption.New)
            {
                newOrders = await Context.Orders.Include(o => o.Location)
                    .Where(o => o.StatusId == (int)OrderStatus.Active
                    //&& o.Date > DateTime.Now
                    && o.Location.CityId == workerSetting.CityId 
                    && workerSetting.DistrictIds.Contains(o.Location.DistrictId)
                    && workerSetting.CategoryIds.Contains(o.CategoryId))
                    .Include(o => o.OrderDetails)
                    .ToListAsync();
                newOrders.Sort();
            }

            List<Order> workOrders = null;
            if (dataOption == UserInformerOption.All || dataOption == UserInformerOption.Work)
            {
                workOrders = await Context.Orders
                    .Include(o => o.User).Include(o => o.Location)
                    .Where(o => o.WorkerId == userWorker.Worker.Id 
                    && o.StatusId == (int)OrderStatus.InProgress)
                    .Include(o => o.OrderDetails)
                    .ToListAsync();
                workOrders.Sort();
            }

            City workerCity = null;
            List<District> workerDistricts = null;
            List<Category> workerCategories = null;
            switch (dataOption)
            {
                case UserInformerOption.All:
                    Context.Cities.Load();
                    Context.Districts.Load();
                    Context.Categories.Where(c => c.ParentId != null).Load();
                    Context.Services.Load();
                    var tmpConcat = newOrders.Concat(workOrders);
                    workerDistricts = tmpConcat.Select(o => o.Location.District).Distinct().ToList();
                    workerCategories = tmpConcat.Select(o => o.Category).Distinct().ToList();
                    break;
                case UserInformerOption.New:
                    Context.Districts.Where(d => d.CityId == workerSetting.CityId).Load();
                    Context.Categories.Where(c => c.ParentId != null && workerSetting.CategoryIds.Contains(c.Id)).Load();
                    Context.Services.Where(s => workerSetting.CategoryIds.Contains(s.CategoryId)).Load();
                    workerDistricts = newOrders.Select(o => o.Location.District).Distinct().ToList();
                    workerCategories = newOrders.Select(o => o.Category).Distinct().ToList();
                    break;
                case UserInformerOption.Work:
                    //Context.Districts.Where(d => d.CityId == workerSetting.CityId).Load();
                    //Context.Categories.Where(c => c.ParentId != null && workerSetting.CategoryIds.Contains(c.Id)).Load();
                    //Context.Services.Where(s => workerSetting.CategoryIds.Contains(s.CategoryId)).Load();
                    Context.Cities.Load();
                    Context.Districts.Load();
                    Context.Categories.Where(c => c.ParentId != null).Load();
                    Context.Services.Load();
                    workerDistricts = workOrders.Select(o => o.Location.District).Distinct().ToList();
                    workerCategories = workOrders.Select(o => o.Category).Distinct().ToList();
                    break;
                case UserInformerOption.Settings:
                    workerCity = await Context.Cities
                    .Where(c => c.Id == workerSetting.CityId)
                    .Include(c => c.Districts)
                    .SingleOrDefaultAsync();
                    workerCategories = await Context.Categories.ToListAsync();
                    break;
                default:
                    break;
            }

            WorkerPageViewModel workerPageData = new WorkerPageViewModel
            {
                UserId = userWorker.Id,
                WorkerId = userWorker.Worker.Id,
                WorkerSetting = workerSetting,
                Categories = workerCategories,
                Districts = workerDistricts,
                City = workerCity,
                OrdersNew = newOrders,
                OrdersInWork = workOrders
            };

            return workerPageData;
        }

        public async Task<OrderDataModel> GetOrderDataModelAsync(int orderId)
        {
            ApplicationUser userWorker = await GetUserWorkerAsync();
            if (userWorker == null)
            {
                return null;
            }

            WorkerSetting workerSetting = await GetWorkerSetting(userWorker);

            Order order = await Context.Orders
                    .Include(o => o.User).Include(o => o.Location).ThenInclude(o => o.District)
                    .Where(o => o.Id == orderId)
                    .Include(o => o.OrderDetails)
                    .SingleOrDefaultAsync();

            Context.Categories.Where(c => c.Id == order.CategoryId).Load();
            Context.Services.Where(s => s.CategoryId == order.CategoryId).Load();

            OrderDataModel orderDM = new OrderDataModel
            {
                Id = order.Id,
                Number = order.Number,
                Date = order.Date.ToString(),
                CategoryName = order.Category.Name,
                ClientCity = order.Location.City.Name,
                ClientDistrict = order.Location.District.Name,
                ClientAddress = order.Location.Address,
                ClientPhone = order.User.PhoneNumber,
                Description = order.Description,
                OrderDetails = order.OrderDetails
                .Select(o => new OrderDetailDataModel { Quantity = o.Quantity, SeviceName = o.Service.Name }).ToList(),
                TotalCost = order.TotalCost
            };

            return orderDM;
        }

        public async Task<List<OrderDataModel>> GetOrdersDataModelAsync(UserInformerOption dataOption)
        {
            ApplicationUser userWorker = await GetUserWorkerAsync();
            if (userWorker == null)
            {
                return null;
            }

            WorkerSetting workerSetting = await GetWorkerSetting(userWorker);
            
            List<Order> newOrders = new List<Order>();
            if (dataOption == UserInformerOption.All || dataOption == UserInformerOption.New)
            {
                newOrders = await Context.Orders.Include(o => o.Location)
                    .Where(o => o.StatusId == (int)OrderStatus.Active
                    && o.Location.CityId == workerSetting.CityId
                    && workerSetting.DistrictIds.Contains(o.Location.DistrictId))
                    .Include(o => o.OrderDetails)
                    .ToListAsync();
            }

            List<Order> workOrders = new List<Order>();
            if (dataOption == UserInformerOption.All || dataOption == UserInformerOption.Work)
            {
                workOrders = await Context.Orders
                    .Include(o => o.User).Include(o => o.Location)
                    .Where(o => o.WorkerId == userWorker.Worker.Id
                    && o.StatusId == (int)OrderStatus.InProgress
                    && o.Location.CityId == workerSetting.CityId)
                    .Include(o => o.OrderDetails)
                    .ToListAsync();
            }

            Context.Districts.Where(d => d.CityId == workerSetting.CityId).Load();
            Context.Categories.Where(c => c.ParentId != null && workerSetting.CategoryIds.Contains(c.Id)).Load();
            Context.Services.Where(s => workerSetting.CategoryIds.Contains(s.CategoryId)).Load();

            List<OrderDataModel> orders = null;
            switch (dataOption)
            {
                case UserInformerOption.All: orders = new List<OrderDataModel>(newOrders.Count + workOrders.Count);
                    foreach (var item in newOrders)
                    {
                        OrderDataModel orderDM = new OrderDataModel
                        {
                            Id = item.Id,
                            Number = item.Number,
                            Date = item.Date.ToString(),
                            CategoryName = item.Category.Name,
                            ClientCity = item.Location.City.Name,
                            ClientDistrict = item.Location.District.Name,
                            ClientAddress = item.Location.Address,
                            ClientPhone = item.User.PhoneNumber,
                            Description = item.Description,
                            OrderDetails = item.OrderDetails.Select(o => new OrderDetailDataModel { Quantity = o.Quantity, SeviceName = o.Service.Name }).ToList(),
                            TotalCost = item.TotalCost
                        };
                        orders.Add(orderDM);
                    }
                    foreach (var item in workOrders)
                    {
                        OrderDataModel orderDM = new OrderDataModel
                        {
                            Id = item.Id,
                            Number = item.Number,
                            Date = item.Date.ToString(),
                            CategoryName = item.Category.Name,
                            ClientCity = item.Location.City.Name,
                            ClientDistrict = item.Location.District.Name,
                            ClientAddress = item.Location.Address,
                            ClientPhone = item.User.PhoneNumber,
                            Description = item.Description,
                            OrderDetails = item.OrderDetails.Select(o => new OrderDetailDataModel { Quantity = o.Quantity, SeviceName = o.Service.Name }).ToList(),
                            TotalCost = item.TotalCost
                        };
                        orders.Add(orderDM);
                    }
                    break;
                case UserInformerOption.New: orders = new List<OrderDataModel>(newOrders.Count);
                    foreach (var item in newOrders)
                    {
                        OrderDataModel orderDM = new OrderDataModel
                        {
                            Id = item.Id,
                            Number = item.Number,
                            Date = item.Date.ToString(),
                            CategoryName = item.Category.Name,
                            ClientCity = item.Location.City.Name,
                            ClientDistrict = item.Location.District.Name,
                            ClientAddress = "",
                            ClientPhone = "",
                            Description = item.Description,
                            OrderDetails = item.OrderDetails.Select(o => new OrderDetailDataModel { Quantity = o.Quantity, SeviceName = o.Service.Name }).ToList(),
                            TotalCost = item.TotalCost
                        };
                        orders.Add(orderDM);
                    }
                    break;
                case UserInformerOption.Work: orders = new List<OrderDataModel>(workOrders.Count);
                    foreach (var item in workOrders)
                    {
                        OrderDataModel orderDM = new OrderDataModel
                        {
                            Id = item.Id,
                            Number = item.Number,
                            Date = item.Date.ToString(),
                            CategoryName = item.Category.Name,
                            ClientCity = item.Location.City.Name,
                            ClientDistrict = item.Location.District.Name,
                            ClientAddress = item.Location.Address,
                            ClientPhone = item.User.PhoneNumber,
                            Description = item.Description,
                            OrderDetails = item.OrderDetails.Select(o => new OrderDetailDataModel { Quantity = o.Quantity, SeviceName = o.Service.Name }).ToList(),
                            TotalCost = item.TotalCost
                        };
                        orders.Add(orderDM);
                    }
                    break;
            };

           
            return orders;
        }

        private async Task<WorkerSetting> GetWorkerSetting(ApplicationUser userWorker)
        {
            WorkerSetting workerSetting;
            try
            {
                workerSetting = userWorker.Worker.WorkerSettingJson.ParseToWorkerSetting();
            }
            catch
            {
                workerSetting = new WorkerSetting
                {
                    //CityId = userWorker.CityId,
                    //CategoryIds = await Context.Categories.Where(c => c.ParentId != null).Select(c => c.Id).ToListAsync(),
                    //DistrictIds = await Context.Districts.Where(d => d.CityId == userWorker.CityId).Select(d => d.Id).ToListAsync()
                    CityId = userWorker.CityId,
                    CategoryIds = new List<int>(),
                    DistrictIds = new List<int>()
                };
                userWorker.Worker.WorkerSettingJson = workerSetting.ToJSON();
                Context.Update(userWorker.Worker);
                await Context.SaveChangesAsync();
            }

            return workerSetting;
        }

    }
}
