using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models;

public partial class Request
{
    [Key]
    [Column("reqid")]
    public int Reqid { get; set; }

    public string Userid { get; set; }
    [ForeignKey("Userid")]

    public virtual ApplicationUser? User { get; set; }

    public string Useridreq { get; set; }
    [ForeignKey("Useridreq")]
    public virtual ApplicationUser? User2 { get; set; }

    public int AnimalId { get; set; }
    [ForeignKey("AnimalId")]
    public virtual Animal? Animal { get; set; }

    public string? Status { get; set; } = "Pending";
}