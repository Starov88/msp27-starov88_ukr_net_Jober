using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jober.Data;
using Jober.Models;
using Jober.Areas.API.Models;
using Jober.Services.Informer;

namespace Jober.Areas.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Order")]
    [Area("api")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<OrderDataModel>> GetOrders()
        {
            PutOrderDataModel data = CheackRequest();
            if (data == null)
            {
                return null;
            }

            UserInformerOption option = UserInformerOption.New;
            if (Request.Query.ContainsKey("option"))
            {
                switch (Request.Query["option"].ToString().ToLower())
                {
                    case "work": option = UserInformerOption.Work; break;
                    case "all": option = UserInformerOption.All; break;
                    default: break;
                }
            }

            UserInformer informer = new UserInformer(_context, Request.Query["guid"]);
            List<OrderDataModel> model = await informer.GetOrdersDataModelAsync(option);

            return model;
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] int id)
        {
            PutOrderDataModel data = CheackRequest();
            if (data == null)
            {
                return BadRequest();
            }

            ApplicationUser user = await GetUserWorker(data);
            if (user == null)
            {
                return Unauthorized();
            }

            var order = await _context.Orders
                .Where(o => o.Id == id)
                .Include(o => o.User)
                .Include(o => o.Category).ThenInclude(c => c.Services)
                .Include(o => o.Location).ThenInclude(l => l.District)
                .Include(o => o.Location).ThenInclude(l => l.City)
                .Include(o => o.OrderDetails)
                .SingleOrDefaultAsync();
            if (order == null)
            {
                return NotFound();
            }

            //_context.Cities.Where(c => c.Id == order.Location.District.CityId).Load();
            //_context.Services.Where(s => s.CategoryId == order.CategoryId).Load();
            //_context.Categories.Where(c => c.Id == order.CategoryId).Load();

            OrderDataModel dataModel = new OrderDataModel
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
                OrderDetails = order.OrderDetails.Select(o => new OrderDetailDataModel { Quantity = o.Quantity, SeviceName = o.Service.Name }).ToList(),
                TotalCost = order.TotalCost
            };

            return Ok(dataModel);
        }

        // PUT: api/Orders/5

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder([FromRoute] int id, [FromBody] PutOrderDataModel data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            data = CheackRequest(data);
            if (data == null)
            {
                return BadRequest();
            }

            ApplicationUser user = await GetUserWorker(data);
            if (user == null)
            {
                return Unauthorized();
            }

            if (!OrderExists(id))
            {
                return NotFound();
            }

            OrderStatus orderStatus = OrderStatus.NaN;
            switch ((OrderStatus)data.Status)
            {
                case OrderStatus.Active: orderStatus = OrderStatus.Active; break;
                case OrderStatus.InProgress: orderStatus = OrderStatus.InProgress; break;
                case OrderStatus.Complete: orderStatus = OrderStatus.Complete; break;
                default: break;
            }

            if (orderStatus == OrderStatus.NaN)
                return Forbid();

            Order order = await _context.Orders.SingleOrDefaultAsync(o => o.Id == id);
            if (order.WorkerId != null && order.WorkerId != data.WorkerId)
            {
                return Forbid();
            }
            if (order.StatusId == (int)orderStatus)
            {
                return NoContent();
            }

            float paySum = GetPaySum((float)order.TotalCost, orderStatus, (OrderStatus)order.StatusId);
            if (paySum != 0)
            {
                if (!CanUserPay(user, paySum))
                {
                    return Forbid();
                }
                user.Balance -= paySum;
                _context.Entry(user).State = EntityState.Modified;
            }

            order.StatusId = (int)orderStatus;
            if (orderStatus == OrderStatus.Active)
            {
                order.WorkerId = null;
            }
            else
            {
                if (orderStatus == OrderStatus.InProgress)
                    order.WorkerId = user.WorkerId;
            }
            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id) || await GetUserWorker(data) == null)
                {
                    return NotFound();
                }
                else
                {
                    return NoContent();
                }
            }

            //if (orderStatus == OrderStatus.Active)
            //{
            //    return Ok();
            //}
            //else
            //{
            //    _context.Entry(order).Collection(o => o.OrderDetails).Load();
            //    _context.Entry(order).Reference(o => o.User).Load();
            //    _context.Entry(order).Reference(o => o.Location).Load();
            //    _context.Entry(order).Reference(o => o.Category).Load();
            //    _context.Entry(order.Location).Reference(l => l.City).Load();
            //    _context.Entry(order.Location).Reference(l => l.District).Load();
            //    _context.Services.Where(s => s.CategoryId == order.CategoryId).Load();

            //    OrderDataModel orderDM = new OrderDataModel
            //    {
            //        Id = order.Id,
            //        Number = order.Number,
            //        Date = order.Date.ToString(),
            //        CategoryName = order.Category.Name,
            //        ClientCity = order.Location.City.Name,
            //        ClientDistrict = order.Location.District.Name,
            //        ClientAddress = order.Location.Address,
            //        ClientPhone = order.User.PhoneNumber,
            //        Description = order.Description,
            //        OrderDetails = order.OrderDetails
            //        .Select(o => new OrderDetailDataModel { Quantity = o.Quantity, SeviceName = o.Service.Name }).ToList(),
            //        TotalCost = order.TotalCost
            //    };
            //    return Ok();
            //}

            return Ok();
        }

        /*[HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }*/

        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }*/

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        private PutOrderDataModel CheackRequest(PutOrderDataModel data = null)
        {
            if (data == null)
            {
                if (!Request.Query.ContainsKey("guid") && !Request.Query.ContainsKey("workerId"))
                {
                    return null;
                }

                int workerId;
                if (!int.TryParse(Request.Query["workerId"], out workerId))
                {
                    return null;
                }
                data = new PutOrderDataModel
                {
                    Guid = Request.Query["guid"],
                    WorkerId = workerId
                };
            }
            else
            {
                if (string.IsNullOrEmpty(data.Guid) || data.WorkerId == default(int))
                {
                    return null;
                }
            }

            return data;
        }

        private async Task<ApplicationUser> GetUserWorker(PutOrderDataModel data)
        {
            if(data == null || data.WorkerId == default(int) || string.IsNullOrEmpty(data.Guid))
            {
                return null;
            }

            ApplicationUser user = await _context.Users.Include(u => u.Worker)
                .Where(u => u.Id == data.Guid && u.WorkerId == data.WorkerId && u.Worker.IsActive == true)
                .Include(u => u.Worker)
                .SingleOrDefaultAsync();

            return user;
        }   

        private float GetPaySum(float orderPrice, OrderStatus newStatus, OrderStatus oldStatus)
        {
            float pay = 0;
            switch (oldStatus)
            {
                case OrderStatus.Active:
                    if(newStatus == OrderStatus.InProgress)
                        pay = orderPrice * 0.1F;
                    break;
                case OrderStatus.Complete:
                    break;
                case OrderStatus.Canceled:
                    break;
                case OrderStatus.Failed:
                    break;
                case OrderStatus.InProgress:
                    if(newStatus == OrderStatus.Active)
                        pay = -(orderPrice * 0.1F / 2);
                    break;
                case OrderStatus.NaN:
                    break;
                default:
                    break;
            }
            return pay;
        }

        private bool CanUserPay(ApplicationUser user, float price)
        {
            if (user.Balance >= price)
            { 
                return true;
            }
            return false;
        }
    }
}