using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatAppReact.Models;

namespace ChatAppReact.Services
{
	public class ChatMessageRepositoryLocal : IChatMessageRepository
	{
		private static ConcurrentBag<ChatMessage> _chatMessages = new ConcurrentBag<ChatMessage>();

		public async Task AddMessage(ChatMessage message)
		{
			await Task.Run(() => _chatMessages.Add(message)); 
		}

		public Task<IEnumerable<ChatMessage>> GetTopMessages(int number = 100)
		{
			return Task.FromResult(_chatMessages.OrderBy(x => x.Date).AsEnumerable());
		}
	}
}
