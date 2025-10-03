using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models;

public partial class Request
{
    [Key]
    [Column("reqid")]
    public int Reqid { get; set; }

    public int Userid { get; set; }
    [ForeignKey("Userid")]
    [InverseProperty("RequestsSent")] // Corresponds to the collection in the User model
    public virtual User? User { get; set; }

    public int Useridreq { get; set; }
    [ForeignKey("Useridreq")]
    [InverseProperty("RequestsReceived")] // Corresponds to the other collection
    public virtual User? User2 { get; set; }

    public int AnimalId { get; set; }
    [ForeignKey("AnimalId")]
    public virtual Animal? Animal { get; set; }

    public string? Status { get; set; }= "Pending";
}