using System.Collections.Generic;
using System.Threading.Tasks;
using ChatApp_ASPdotNETCore_SignalR.Models;

namespace ChatApp_ASPdotNETCore_SignalR.Infrastructure.Respository
{
    public interface IChatRepository
    {
        Chat GetChat(int id);
        Task CreateRoom(string name, string userId);
        Task JoinRoom(int chatId, string userId);
        IEnumerable<Chat> GetChats(string userId);
        Task<int> CreatePrivateRoom(string rootId, string targetId);
        IEnumerable<Chat> GetPrivateChats(string userId);
        Task<Message> CreateMessage(int chatId, string message, string userId);
        Task UpdateUser(string userid, User user);
    }
}