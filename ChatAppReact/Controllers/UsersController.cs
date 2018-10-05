using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ChatAppReact.Models;
using ChatAppReact.User;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppReact.Controllers
{
	[Route("api/[controller]")]
	public class UsersController : Controller
    {
		private readonly IUserTracker _userTracker;

		public UsersController(IUserTracker userTracker)
		{
			_userTracker = userTracker;
		}

		[HttpGet("exists")]
		public IActionResult CheckIfUserExist([FromQuery] string name)
		{
			if (_userTracker.UsersOnline().FirstOrDefault(u => u.Name == name) != null)
			{
				return Conflict($"User with name {name} already exists");
			}
			return NoContent();
		}

		[HttpGet("[action]")]
		public IEnumerable<UserDetails> LoggedOnUsers()
		{
			return _userTracker.UsersOnline();
		}
    }
}
