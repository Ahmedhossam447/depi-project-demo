using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models;

public partial class Animal
{
    public int AnimalId { get; set; }

    public string? Name { get; set; }

    public byte? Age { get; set; }

    public string? Type { get; set; }

    public string? Photo { get; set; }

    public string? Userid { get; set; }
    [ForeignKey("Userid")]
    public virtual IdentityUser? User { get; set; }

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

}
