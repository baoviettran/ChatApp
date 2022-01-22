using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp_ASPdotNETCore_SignalR.Database;
using ChatApp_ASPdotNETCore_SignalR.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp_ASPdotNETCore_SignalR.Infrastructure.Respository
{
    public class ChatRepository : IChatRepository
    {
        private ChatAppDbContext _ctx;

        public ChatRepository(ChatAppDbContext ctx) => _ctx = ctx;

        public async Task<Message> CreateMessage(int chatId, string message, string userId)
        {
            var Message = new Message
            {
                ChatId = chatId,
                Text = message,
                Name = userId,
                Timestamp = DateTime.Now
            };

            _ctx.Messages.Add(Message);
            await _ctx.SaveChangesAsync();

            return Message;
        }

        public async Task<int> CreatePrivateRoom(string rootId, string targetId)
        {
            var chat = new Chat
            {
                Type = ChatType.Private
            };

            chat.Users.Add(new ChatUser
            {
                UserId = targetId
            });

            chat.Users.Add(new ChatUser
            {
                UserId = rootId
            });

            _ctx.Chats.Add(chat);

            await _ctx.SaveChangesAsync();

            return chat.Id;
        }

        public async Task CreateRoom(string name, string userId)
        {
            var chat = new Chat
            {
                Name = name,
                Type = ChatType.Room
            };

            chat.Users.Add(new ChatUser
            {
                UserId = userId,
                Role = UserRole.Admin
            });

            _ctx.Chats.Add(chat);

            await _ctx.SaveChangesAsync();
        }

        public Chat GetChat(int id)
        {
            return _ctx.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Chat> GetChats(string userId)
        {
            return _ctx.Chats
                .Include(x => x.Users)
                .Where(x => !x.Users
                    .Any(y => y.UserId == userId))
                .ToList();
        }

        public IEnumerable<Chat> GetPrivateChats(string userId)
        {
            return _ctx.Chats
                   .Include(x => x.Users)
                       .ThenInclude(x => x.User)
                   .Where(x => x.Type == ChatType.Private
                       && x.Users
                           .Any(y => y.UserId == userId))
                   .ToList();
        }

        public async Task JoinRoom(int chatId, string userId)
        {
            var chatUser = new ChatUser
            {
                ChatId = chatId,
                UserId = userId,
                Role = UserRole.Member
            };

            _ctx.ChatUsers.Add(chatUser);

            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateUser(string userid, User user)
        {
            var entity = _ctx.Users.FirstOrDefault(item => item.Id == userid);
            if (entity != null)
            {
                entity.address = user.address;
                entity.firstName = user.firstName;
                entity.lastName= user.lastName;
                entity.phone = user.phone;
                entity.Email = user.Email;
                _ctx.SaveChanges();
            }
        }
    }
}