using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace test.Models;

[Table("medical_record")]
public partial class MedicalRecord
{
    [Key]
    [Column("recordid")]
    public int Recordid { get; set; }

    [Column("animalid")]
    public int? Animalid { get; set; }

    [Column("injurys")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Injurys { get; set; }

    [Column("status")]
    [StringLength(20)]
    [Unicode(false)]
    public string? Status { get; set; }

    [ForeignKey("Animalid")]
    [InverseProperty("MedicalRecords")]
    public virtual Animal? Animal { get; set; }

    [InverseProperty("Medical")]
    public virtual ICollection<VaccinationNeeded> VaccinationNeededs { get; set; } = new List<VaccinationNeeded>();
}
