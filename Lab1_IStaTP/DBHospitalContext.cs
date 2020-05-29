using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Lab1_IStaTP
{
    public partial class DBHospitalContext : DbContext
    {
        public DBHospitalContext()
        {
        }

        public DBHospitalContext(DbContextOptions<DBHospitalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Compexities> Compexities { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<Diseases> Diseases { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Neighbourhoods> Neighbourhoods { get; set; }
        public virtual DbSet<PatientRegistration> PatientRegistration { get; set; }
        public virtual DbSet<Patients> Patients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=BOGRCH\\SQLEXPRESS; Database=DBHospital; Trusted_Connection=true; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryNaming).IsUnicode(false);
            });

            modelBuilder.Entity<Compexities>(entity =>
            {
                entity.HasKey(e => e.ComplexityId);

                entity.Property(e => e.ComplexityId).HasColumnName("ComplexityID");

                entity.Property(e => e.ComplexityNaming)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Departments>(entity =>
            {
                entity.HasKey(e => e.DepartmentId);

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.ChiefOfDepartment)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DepartmentNaming)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Diseases>(entity =>
            {
                entity.HasKey(e => e.DiseaseId)
                    .HasName("PK_Table_1");

                entity.Property(e => e.DiseaseId).HasColumnName("DiseaseID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.DiseaseNaming)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Diseases)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Diseases_Categories");
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_Employees_Departments");
            });

            modelBuilder.Entity<Neighbourhoods>(entity =>
            {
                entity.HasKey(e => e.NeighbourhoodId);

                entity.Property(e => e.NeighbourhoodId).HasColumnName("NeighbourhoodID");

                entity.Property(e => e.NeighbourhoodNaming)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PatientRegistration>(entity =>
            {
                entity.HasKey(e => e.AccountId);

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.ComplexityId).HasColumnName("ComplexityID");

                entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");

                entity.Property(e => e.DiseaseId).HasColumnName("DiseaseID");

                entity.Property(e => e.PatientId).HasColumnName("PatientID");

                entity.Property(e => e.PrdateOfDischarge)
                    .HasColumnName("PRDateOfDischarge")
                    .HasColumnType("date");

                entity.Property(e => e.PrdateOfStart)
                    .HasColumnName("PRDateOfStart")
                    .HasColumnType("date");

                entity.HasOne(d => d.Complexity)
                    .WithMany(p => p.PatientRegistration)
                    .HasForeignKey(d => d.ComplexityId)
                    .HasConstraintName("FK_PatientRegistration_Compexities");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.PatientRegistration)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_PatientRegistration_Departments");

                entity.HasOne(d => d.Disease)
                    .WithMany(p => p.PatientRegistration)
                    .HasForeignKey(d => d.DiseaseId)
                    .HasConstraintName("FK_PatientRegistration_Diseases");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientRegistration)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FK_PatientRegistration_Patients");
            });

            modelBuilder.Entity<Patients>(entity =>
            {
                entity.HasKey(e => e.PatientId);

                entity.Property(e => e.PatientId).HasColumnName("PatientID");

                entity.Property(e => e.Paddress)
                    .HasColumnName("PAddress")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PdateOfBirth)
                    .HasColumnName("PDateOfBirth")
                    .HasColumnType("date");

                entity.Property(e => e.Pname)
                    .HasColumnName("PName")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Pneighbourhood).HasColumnName("PNeighbourhood");

                entity.Property(e => e.Psurname)
                    .HasColumnName("PSurname")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.PneighbourhoodNavigation)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.Pneighbourhood)
                    .HasConstraintName("FK_Patients_Neighbourhoods");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
