using Microsoft.EntityFrameworkCore;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace test.Models
{
    public class User
    {
        [Key]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }
        [Column("username")]
        public string? Username { get; set; }
        [Column("email")]
        public string? Email { get; set; }
        [Column("password")]
        public string? Password { get; set; }
        [Column("role")]
        public string Role { get; set; }= "user";
        [Column("phonenumber")]
        public string? Phonenumber { get; set; }
        public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();
        public virtual ICollection<Product> products { get; set; } = new List<Product>();

        [InverseProperty("User")] // Corresponds to the navigation property in Request
        public virtual ICollection<Request> RequestsSent { get; set; } = new List<Request>();

        [InverseProperty("User2")] // Corresponds to the other navigation property
        public virtual ICollection<Request> RequestsReceived { get; set; } = new List<Request>();

        
    }
}
