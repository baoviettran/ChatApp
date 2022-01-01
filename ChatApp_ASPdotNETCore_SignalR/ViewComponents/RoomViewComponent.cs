using System.Linq;
using System.Security.Claims;
using ChatApp_ASPdotNETCore_SignalR.Database;
using ChatApp_ASPdotNETCore_SignalR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.ViewComponents
{
    public class RoomViewComponent : ViewComponent
    {
        private ChatAppDbContext _ctx;

        public RoomViewComponent(ChatAppDbContext ctx)
        {
            _ctx = ctx;
        }

        public IViewComponentResult Invoke()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var chats = _ctx.ChatUsers
            .Include(x => x.Chat)
            .Where(x => x.UserId == userId)
            .Select(x => x.Chat)
            .ToList();


            return View(chats);
        }
    }
}