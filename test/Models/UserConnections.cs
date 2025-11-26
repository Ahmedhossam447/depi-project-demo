using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models
{
    public class UserConnections
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual IdentityUser? User { get; set; }
        public string ConnectionId { get; set; }

    }
}
