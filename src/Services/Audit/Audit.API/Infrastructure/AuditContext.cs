using Audit.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Audit.API.Infrastructure
{
    public class AuditContext
    {
        private readonly IMongoDatabase _database = null;

        public AuditContext(IOptions<AuditSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Message> Messages => _database.GetCollection<Message>("Message");
    }
}