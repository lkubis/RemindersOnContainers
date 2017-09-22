using System.Threading.Tasks;
using Audit.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Audit.API.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AuditContext _context;

        public MessageRepository(IOptions<AuditSettings> settings)
        {
            _context = new AuditContext(settings);
        }

        public async Task AddMessage(Message message)
        {
            await _context.Messages.InsertOneAsync(message);
        }

        public IMongoCollection<Message> FindAll()
        {
            return _context.Messages;
        }
    }
}