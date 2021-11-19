﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ElectronicMedicalRecords.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CP24Team08Entities : DbContext
    {
        public CP24Team08Entities()
            : base("name=CP24Team08Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Assort> Assorts { get; set; }
        public virtual DbSet<Clinical> Clinicals { get; set; }
        public virtual DbSet<Diagnostic> Diagnostics { get; set; }
        public virtual DbSet<DiagnosticsCategory> DiagnosticsCategories { get; set; }
        public virtual DbSet<DongMau> DongMaus { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<HomeTown> HomeTowns { get; set; }
        public virtual DbSet<InformationExamination> InformationExaminations { get; set; }
        public virtual DbSet<MedicalTestsPrescription> MedicalTestsPrescriptions { get; set; }
        public virtual DbSet<Medication> Medications { get; set; }
        public virtual DbSet<MedicationCategory> MedicationCategories { get; set; }
        public virtual DbSet<Nation> Nations { get; set; }
        public virtual DbSet<Nation1> Nation1 { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<PatientStatu> PatientStatus { get; set; }
        public virtual DbSet<Prescription> Prescriptions { get; set; }
        public virtual DbSet<Prescription_Detail> Prescription_Detail { get; set; }
        public virtual DbSet<Religion> Religions { get; set; }
        public virtual DbSet<Subclinical> Subclinicals { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<XetNghiemMau> XetNghiemMaus { get; set; }
        public virtual DbSet<CTMau> CTMaus { get; set; }
        public virtual DbSet<Detail_CTMau> Detail_CTMau { get; set; }
        public virtual DbSet<SinhHoaMau> SinhHoaMaus { get; set; }
        public virtual DbSet<Detail_SinhHoaMau> Detail_SinhHoaMau { get; set; }
        public virtual DbSet<Detail_Urine> Detail_Urine { get; set; }
        public virtual DbSet<Urine> Urines { get; set; }
    }
}
