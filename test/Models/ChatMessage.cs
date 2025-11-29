using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace test.Models
{
    public class ChatMessage
    {
        public int id { get; set; }
        
        public String SenderId { get; set; }
        [ForeignKey("SenderId")]
        public IdentityUser? User { get; set; }


        public string ReceiverId { get; set; }
        [ForeignKey("ReceiverId")]
        public IdentityUser? Receiver { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
        public int read { get; set; }
        
        public int? AnimalId { get; set; }
        [ForeignKey("AnimalId")]
        public Animal? Animal { get; set; }
    }
}
