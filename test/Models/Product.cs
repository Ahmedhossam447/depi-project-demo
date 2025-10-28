using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace test.Models;

public partial class Product
{
    [Key]
    [Column("productid")]
    public int Productid { get; set; }

    public string? Userid { get; set; }
    [ForeignKey("Userid")]
    public IdentityUser? User { get; set; }

    [Column("type")]
    [StringLength(50)]
    [Unicode(false)]
    public string? Type { get; set; }

    [Column("quantity")]
    public int? Quantity { get; set; }

    [Column("price")]
    public int? Price { get; set; }

    [Column("disc")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Disc { get; set; }

    [Column("photo")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Photo { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; }


}
