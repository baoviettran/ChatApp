using ChatApp_ASPdotNETCore_SignalR.Models;
using ChatApp_ASPdotNETCore_SignalR.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ChatApp_ASPdotNETCore_SignalR.Infrastructure;
using ChatApp_ASPdotNETCore_SignalR.Infrastructure.Respository;
using Microsoft.AspNetCore.SignalR;
using ChatApp_ASPdotNETCore_SignalR.Hubs;
using Microsoft.AspNetCore.Identity;

namespace ChatApp_ASPdotNETCore_SignalR.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private IChatRepository _repo;
        private UserManager<User> _userManager;
        public HomeController(
            IChatRepository repo, 
            UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;

        }
       
        public IActionResult Index()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            ViewData["nameUser"] = userName;
            var chats = _repo.GetChats(GetUserId());
            //var user = await _userManager.FindByNameAsync(username);

            return View(chats);
        }

        public IActionResult Find([FromServices] ChatAppDbContext ctx)
        {
            var users = ctx.Users
                .Where(x => x.Id != User.GetUserId())
                .ToList();

            return View(users);
        }

        public async Task<IActionResult> GetUserByName(string name)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(name);
                ViewData["nameUser"] = user;
                return Ok(user);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }
        public IActionResult Private()
        {
            var chats = _repo.GetPrivateChats(GetUserId());

            return View(chats);
        }

        public async Task<IActionResult> CreatePrivateRoom(string userId)
        {
            var id = await _repo.CreatePrivateRoom(GetUserId(), userId);

            return RedirectToAction("Chat", new { id });
        }

        [HttpGet("{id}")]
        public IActionResult Chat(int id)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            ViewData["nameUser"] = userName;
            return View(_repo.GetChat(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(string name)
        {
            await _repo.CreateRoom(name, GetUserId());
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> JoinRoom(int id)
        {
            await _repo.JoinRoom(id, GetUserId());

            return RedirectToAction("Chat", "Home", new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(
            int roomId,
            string message,
            [FromServices] IHubContext<ChatHub> chat)
        {
            var Message = await _repo.CreateMessage(roomId, message, User.Identity.Name);

            await chat.Clients.Group(roomId.ToString())
                .SendAsync("RecieveMessage", new
                {
                    Text = Message.Text,
                    Name = Message.Name,
                    Timestamp = Message.Timestamp.ToString("dd/MM/yyyy hh:mm:ss")
                });

            return Ok();
        }
    }
}
