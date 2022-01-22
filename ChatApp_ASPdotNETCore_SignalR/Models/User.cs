using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ChatApp_ASPdotNETCore_SignalR.Models
{
    public class User : IdentityUser
    {
        //public ICollection<ChatUser> Chats { get; set; }
        public User() : base()
        {
            Chats = new List<ChatUser>();
        }
        public ICollection<ChatUser> Chats { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int phone { get; set; }
        public string address { get; set; }
    }
}