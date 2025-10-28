using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace test.Models;

[Table("vaccination_needed")]
public partial class VaccinationNeeded
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("medicalid")]
    public int? Medicalid { get; set; }
    [ForeignKey("Medicalid")]
    public virtual MedicalRecord? MedicalRecord { get; set; }

    [Column("vaccine_name")]
    [StringLength(255)]
    [Unicode(false)]
    public string? VaccineName { get; set; }
}