using System.ComponentModel.DataAnnotations;

namespace ChatApp_ASPdotNETCore_SignalR.Models
{
    public class userDetailcs
    {
        [Key]
        public string UserId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int phone { get; set; }
        public string address { get; set; }

        public string email { get; set; }
    }
}
