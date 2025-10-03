using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace test.Models;

public partial class Animal
{
    [Key]
    [Column("animalID")]
    public int AnimalId { get; set; }

    [Column("name", TypeName = "text")]
    public string? Name { get; set; }

    [Column("age")]
    public byte? Age { get; set; }

    [Column("type")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Type { get; set; }

    public int? Userid { get; set; }
    [ForeignKey("Userid")]
    public User? User { get; set; }

    [Column("photo")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Photo { get; set; }

    [InverseProperty("Animal")]
    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();



}
