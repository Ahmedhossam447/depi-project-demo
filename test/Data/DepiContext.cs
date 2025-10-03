﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using test.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace test.Data;

public partial class DepiContext : DbContext
{
    public DepiContext()
    {
    }

    public DepiContext(DbContextOptions<DepiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Animal> Animals { get; set; }
    public virtual DbSet<MedicalRecord> MedicalRecords { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VaccinationNeeded> VaccinationNeededs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=AHMED\\SQLEXPRESS;Initial Catalog=depi;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // --- 3. Call the base method FIRST ---
        // This is the most critical change. It configures all the Identity tables correctly.
        base.OnModelCreating(modelBuilder);

        // --- 4. Keep your custom entity configurations ---
        modelBuilder.Entity<Animal>(entity =>
        {
            entity.HasKey(e => e.AnimalId).HasName("PK__Animals__6874563112ABE582");
        });

        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(e => e.Recordid).HasName("PK__medical___D82414B603C3C68E");
            entity.HasOne(d => d.Animal).WithMany(p => p.MedicalRecords).HasConstraintName("anim_rec_fk");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Productid).HasName("PK__Products__2D172D323F6C272B");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Reqid).HasName("PK__Requests__20C3720149F8AC2F");
        });

        modelBuilder.Entity<VaccinationNeeded>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__vaccinat__3213E83F846723D8");
            entity.HasOne(d => d.Medical).WithMany(p => p.VaccinationNeededs).HasConstraintName("vac_rec_fk");
        });
    } }
