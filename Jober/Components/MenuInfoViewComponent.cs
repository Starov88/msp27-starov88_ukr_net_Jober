using Jober.Data;
using Jober.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jober.Components
{
    public class MenuInfoViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MenuInfoViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            return new HtmlContentViewComponentResult(new HtmlString(HtmlMenuGenerator()));
        }

        private string HtmlMenuGenerator(string activMenuItemId = "")
        {
            int servicesCount = _context.Services.Count();
            int workersOnline = new Random().Next(10, 100);
            float userBalance = 0;
            if (User != null && User.IsInRole("jober"))
            {
                string userId = _userManager.GetUserId(this.UserClaimsPrincipal);
                userBalance = _context.Users.Where(u => u.Id == userId).Select(u => u.Balance).SingleOrDefault();
                
            }

            StringBuilder html = new StringBuilder($"<div id='j-infobox'>");
            html.Append($"<span class='j-bg-blue'><span class='glyphicon glyphicon-cog' aria-hidden='true'></span></span>");
            html.Append($"<span class='j-bg-grey' title='Количество доступных сервисов'>{servicesCount}</span>");
            if(User != null && User.IsInRole("jober"))
            {
                html.Append($"<span class='j-bg-grey' title='Ваш баланс'>{userBalance.ToString("F2")}</span>");
                html.Append($"<span class='j-bg-green'><span class='glyphicon glyphicon-piggy-bank' aria-hidden='true'></span></span>");
            }
            else
            {
                html.Append($"<span class='j-bg-grey' title='Исполнителей в сети'>{workersOnline}</span>");
                html.Append($"<span class='j-bg-green'><span class='glyphicon glyphicon glyphicon-user' aria-hidden='true'></span></span>");
            }
            
            html.Append($"</div>");

            return html.ToString();

            
        }

    }
}
