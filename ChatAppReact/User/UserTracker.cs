using ChatAppReact.Hubs;
using ChatAppReact.Models;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ChatAppReact.User
{
	public class UserTracker : IUserTracker
	{
		private readonly IHubContext<ChatHub> _chatHubContext;
		private readonly IDatabase _cache;

		public UserTracker(IHubContext<ChatHub> chatHubContext)
		{
			_chatHubContext = chatHubContext;
			_cache = RedisConnector.Connection.GetDatabase();
		}

		public void AddUser(string sid, string name)
		{
			if (!_cache.HashExists("Users", $"{sid}"))
			{
				var user = new UserDetails
				{
					Id = sid,
					Name = name
				};
				_cache.HashSet("Users", sid, name);
				_chatHubContext.Clients.All.SendAsync("UserLoggedOn", user);
			}
		}

		public void RemoveUser(string sid)
		{
			string name = _cache.HashGet("Users", sid);
			if (!string.IsNullOrEmpty(name))
			{
				var user = new UserDetails
				{
					Id = sid,
					Name = name
				};
				_cache.HashDelete("Users", sid);
				_chatHubContext.Clients.All.SendAsync("UserLoggedOff", user);
			}
		}

		public IEnumerable<UserDetails> UsersOnline()
		{
			List<UserDetails> users = new List<UserDetails>();
			users = _cache.HashGetAll("Users")?.Select(u => new UserDetails()
			{
				Id = u.Name,
				Name = u.Value
			}).ToList();

			return users;
		}
	}
}