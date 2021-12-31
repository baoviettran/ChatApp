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
            //var userid = httpcontext.user.findfirst(claimtypes.nameidentifier).value;

            var chats = _ctx.Chats.ToList();
                //.Include(x => x.Chat)
                //.where(x => x.userid == userid
                //    && x.chat.type == chattype.room)
                //.select(x => x.chat)
                //.tolist();

            return View(chats);
        }
    }
}