using System.Collections.Generic;
using System.Threading.Tasks;
using test.Models;

namespace test.Interfaces
{
    public interface IContact
    {
        Task AddMessageAsync(ContactMessage message);
        Task<List<ContactMessage>> GetAllMessagesAsync();
    }
}
