using ChatAppReact.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatAppReact.Services
{
    public interface IChatMessageRepository
    {
        Task AddMessage(ChatMessage message);
        Task<IEnumerable<ChatMessage>> GetTopMessages(int number = 100);
    }
}
