using ChatAppReact.Hubs;
using ChatAppReact.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ChatAppReact.User
{
	public class UserTracker : IUserTracker
	{
		private static ConcurrentBag<UserDetails> joinedUsers = new ConcurrentBag<UserDetails>();

		private IHubContext<ChatHub> _chatHubContext;

		public UserTracker(IHubContext<ChatHub> chatHubContext)
		{
			_chatHubContext = chatHubContext;
		}

		public void AddUser(string sid, string name)
		{
			if (!joinedUsers.Any(x => x.Id == sid))
			{
				var user = new UserDetails
				{
					Id = sid,
					Name = name
				};
				joinedUsers.Add(user);
				_chatHubContext.Clients.All.SendAsync("UserLoggedOn", user);
			}
		}

		public void RemoveUser(string sid)
		{
			var user = joinedUsers.FirstOrDefault(x => x.Id == sid);
			if (user != null)
			{
				joinedUsers.ToList().Remove(user);
				_chatHubContext.Clients.All.SendAsync("UserLoggedOff", user);
			}
		}

		public IEnumerable<UserDetails> UsersOnline()
		{
			return joinedUsers;
		}
	}
}