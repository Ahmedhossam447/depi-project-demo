using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using test.Data;
using test.Interfaces;
using test.Models;

namespace test.Repository
{
    public class ContactRepository : IContact
    {
        private readonly DepiContext _context;

        public ContactRepository(DepiContext context)
        {
            _context = context;
        }

        public async Task AddMessageAsync(ContactMessage message)
        {
            await _context.ContactMessages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ContactMessage>> GetAllMessagesAsync()
        {
            return await _context.ContactMessages.OrderByDescending(m => m.SubmittedAt).ToListAsync();
        }
    }
}
