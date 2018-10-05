using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAppReact.User
{
	public class RedisConnector
	{
		private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
		{
			Console.WriteLine($"Create Redis connection to {redisHost}");
			return ConnectionMultiplexer.Connect($"{redisHost}:6380,password={redisPw},ssl=True,abortConnect=False,synctimeout=3000,connectTimeout=10000");
		});

		public static string redisHost;
		public static string redisPw;

		public static ConnectionMultiplexer Connection
		{
			get
			{
				return lazyConnection.Value;
			}
		}
	}
}
