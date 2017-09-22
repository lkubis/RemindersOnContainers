using System.Threading.Tasks;
using Audit.API.Models;
using MongoDB.Driver;

namespace Audit.API.Infrastructure.Repositories
{
    public interface IMessageRepository
    {
        Task AddMessage(Message message);
        IMongoCollection<Message> FindAll();
    }
}