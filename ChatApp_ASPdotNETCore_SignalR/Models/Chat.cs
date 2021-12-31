using System.Collections.Generic;

namespace ChatApp_ASPdotNETCore_SignalR.Models
{
    public class Chat
    {
        public Chat()
        {
            Messages = new List<Message>();
            Users = new List<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ChatType Type { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<User> Users { get; set; }
    }
}