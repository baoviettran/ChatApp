using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp_ASPdotNETCore_SignalR.Infrastructure
{
    public class BaseController : Controller
    {
        protected string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}