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
        private readonly IUserTracker _userTracker;

        public ChatController(IChatService chatService, IUserTracker userTracker)
        {
            _chatService = chatService;
            _userTracker = userTracker;
        }

		[HttpGet("[action]")]
        public IEnumerable<UserDetails> LoggedOnUsers()
        {
            return _userTracker.UsersOnline();
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<ChatMessage>> InitialMessages()
        {
            return await _chatService.GetAllInitially();
        }
    }
}
