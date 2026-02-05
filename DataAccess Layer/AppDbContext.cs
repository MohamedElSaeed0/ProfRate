using Microsoft.EntityFrameworkCore;
using ProfRate.Entities;

namespace ProfRate.Data
{
    // الـ DbContext - بيربط الـ Entities بالـ Database
    public class AppDbContext : DbContext
    {
        // Constructor - بياخد الـ Options من Program.cs
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // الجداول في الـ Database
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<StudentSubject> StudentSubjects { get; set; }
        public DbSet<LecturerSubject> LecturerSubjects { get; set; }

        // تحديد العلاقات بين الجداول
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure Username is Unique for Students
            modelBuilder.Entity<Student>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Performance: Index for sorting and searching by name
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.FirstName);
            
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.LastName);

            // Ensure Username is Unique for Lecturers
            modelBuilder.Entity<Lecturer>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Performance: Index for sorting and searching by name
            modelBuilder.Entity<Lecturer>()
                .HasIndex(l => l.FirstName);
            
            modelBuilder.Entity<Lecturer>()
                .HasIndex(l => l.LastName);

            // Admin -> Students (One-to-Many)
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Admin)
                .WithMany(a => a.Students)
                .HasForeignKey(s => s.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            // Admin -> Lecturers (One-to-Many)
            modelBuilder.Entity<Lecturer>()
                .HasOne(l => l.Admin)
                .WithMany(a => a.Lecturers)
                .HasForeignKey(l => l.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            // Admin -> Questions (One-to-Many)
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Admin)
                .WithMany(a => a.Questions)
                .HasForeignKey(q => q.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            // Evaluation relationships
            modelBuilder.Entity<Evaluation>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Evaluations)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Evaluation>()
                .HasOne(e => e.Lecturer)
                .WithMany(l => l.Evaluations)
                .HasForeignKey(e => e.LecturerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Evaluation>()
                .HasOne(e => e.Question)
                .WithMany(q => q.Evaluations)
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Evaluation>()
                .HasOne(e => e.Subject)
                .WithMany(s => s.Evaluations)
                .HasForeignKey(e => e.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // StudentSubject (Many-to-Many junction table)
            modelBuilder.Entity<StudentSubject>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.StudentSubjects)
                .HasForeignKey(ss => ss.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentSubject>()
                .HasOne(ss => ss.Subject)
                .WithMany(s => s.StudentSubjects)
                .HasForeignKey(ss => ss.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentSubject>()
                .HasOne(ss => ss.Lecturer)
                .WithMany()
                .HasForeignKey(ss => ss.LecturerId)
                .OnDelete(DeleteBehavior.Restrict);

            // LecturerSubject (Many-to-Many junction table)
            modelBuilder.Entity<LecturerSubject>()
                .HasOne(ls => ls.Lecturer)
                .WithMany(l => l.LecturerSubjects)
                .HasForeignKey(ls => ls.LecturerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LecturerSubject>()
                .HasOne(ls => ls.Subject)
                .WithMany(s => s.LecturerSubjects)
                .HasForeignKey(ls => ls.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
