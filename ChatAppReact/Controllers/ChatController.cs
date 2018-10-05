using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChatAppReact.Models;
using ChatAppReact.Services;
using ChatAppReact.User;

namespace ChatApp.Controllers
{
    [Route("api/[controller]")]
	public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ChatMessage>> InitialMessages()
        {
            return await _chatService.GetAllInitially();
        }
    }
}
