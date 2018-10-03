using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using ChatAppReact.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAppReact.Services
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly string _chattableName;
        private readonly CloudTableClient _tableClient;
        private readonly IConfiguration _configuration;

        public ChatMessageRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            var accountName = configuration.GetSection("StorageAccount:accountName").Value;
            var accountKey = configuration.GetSection("StorageAccount:accountKey").Value;
            _chattableName = _configuration.GetSection("StorageAccount:chatMessagesTable").Value;

            var storageCredentials = new StorageCredentials(accountName, accountKey);
            var storageAccount = new CloudStorageAccount(storageCredentials, true);
            _tableClient = storageAccount.CreateCloudTableClient();
        }

        public async Task<IEnumerable<ChatMessage>> GetTopMessages(int number = 100)
        {
            var table = _tableClient.GetTableReference(_chattableName);

            // Create the table if it doesn't exist.
            await table.CreateIfNotExistsAsync();

            string filter = TableQuery.GenerateFilterCondition(
                "PartitionKey",
                QueryComparisons.Equal,
                "chatmessages");

            var query = new TableQuery<ChatMessageTableEntity>()
                .Where(filter)
                .Take(number);

            var entities = await table.ExecuteQuerySegmentedAsync(query, null);

            var result = entities.Results.Select(entity =>
                new ChatMessage
                {
                    Id = entity.RowKey,
                    Date = entity.Timestamp,
                    Message = entity.Message,
                    Sender = entity.Sender
                }).OrderBy(m => m.Date);

            return result;
        }

        public async Task AddMessage(ChatMessage message)
        {
            var table = _tableClient.GetTableReference(_chattableName);

            // Create the table if it doesn't exist.
            await table.CreateIfNotExistsAsync();

            var chatMessage = new ChatMessageTableEntity(message.Id)
            {
                Message = message.Message,
                Sender = message.Sender
            };

            TableOperation insertOperation = TableOperation.Insert(chatMessage);

            // Execute the insert operation.
            var result = await table.ExecuteAsync(insertOperation);
        }

    }
}
