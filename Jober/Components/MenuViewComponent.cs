using Jober.Data;
using Jober.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jober.Components
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MenuViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            if (this.Request.Query.ContainsKey("category"))
            {
                return new HtmlContentViewComponentResult(new HtmlString(HtmlMenuGenerator(Request.Query["category"])));
            }
            if (HttpContext.Items.ContainsKey("category"))
            {
                return new HtmlContentViewComponentResult(new HtmlString(HtmlMenuGenerator(HttpContext.Items["category"].ToString())));
            }
            return new HtmlContentViewComponentResult(new HtmlString(HtmlMenuGenerator()));
        }

        private string HtmlMenuGenerator(string activMenuItemId = "")
        {
            var categories = _context.Categories.ToList();
            List<int> activeMenuItems = GetActiveMenuItems(categories, activMenuItemId);

            string toggleMenuHTML = $"<div id='j-toggle-menu' class='row visible-xs visible-sm j-bg-grey'><span class='glyphicon glyphicon-menu-hamburger' aria-hidden='true'></span><span>Список категорий</span></div>";

            StringBuilder html = new StringBuilder(toggleMenuHTML + $"<ul id=\"j-side-menu\" class=\"nav nav-pills nav-stacked hidden-xs hidden-sm\">");
            foreach(var item in categories.Where(c => c.ParentId == null))
            {
                string addClass = activeMenuItems.Contains(item.Id) ? "active j-side-menu-item" : "j-side-menu-item";
                html.Append($"<li class=\"{addClass}\" data-category_id=\"{item.Id}\"><a>{item.Name}</a></li>");

                addClass = activeMenuItems.Contains(item.Id) ? "j-submenu" : "j-submenu j-submenu-hidden";
                html.Append($"<ul class=\"nav nav-pills nav-stacked {addClass}\">");

                foreach (var subItem in categories.Where(c => c.ParentId == item.Id))
                {
                    addClass = activeMenuItems.Contains(subItem.Id) ? "active j-submenu-item" : "j-submenu-item";

                    html.Append($"<li class=\"{addClass}\" data-category_id=\"{subItem.Id}\"><a href=\"../home/order?category={subItem.Id}\">{subItem.Name}</a></li>");
                }

                html.Append($"</ul>");
            }

            html.Append($"</ul>");

            return html.ToString();

            //html.Append($"<li>Request.PathBase - {this.Request.PathBase}</li>");
            //html.Append($"<li>Request.Path - {this.Request.Path}</li>");
            //html.Append($"<li>Request.Method - {this.Request.Method}</li>");
            //html.Append($"<li>Request.Query.ContainsKey - {this.Request.Query.ContainsKey("id")}</li>");
            //if(this.Request.Query.ContainsKey("id"))
            //    html.Append($"<li>Request.Query[id] - {this.Request.Query["id"]}</li>");
            //html.Append($"<li>{this.Url.RouteUrl("", new { guid = "qqq", name = "Jo" })}</li>");
            //html.Append($"<li>User.Identity.Name - {this.User.Identity.Name}</li>");
            //html.Append($"<li>User.IsInRole(admin) - {this.User.IsInRole("admin")}</li>");
        }

        private List<int> GetActiveMenuItems(IEnumerable<Category> categories, string activMenuId)
        {
            if(string.IsNullOrEmpty(activMenuId))
                return new List<int>();

            int activId;
            if(int.TryParse(activMenuId, out activId))
            {
                Category activCategory = categories.Where(c => c.Id == activId).FirstOrDefault();
                if(activCategory != null)
                {
                    if(activCategory.ParentId != null)
                        return new List<int> { activCategory.Id, (int)activCategory.ParentId };
                    else
                        return new List<int> { activCategory.Id };
                }
            }
            
            return new List<int>();
        }
    }
}
