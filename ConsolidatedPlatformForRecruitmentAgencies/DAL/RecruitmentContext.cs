using ConsolidatedPlatformForRecruitmentAgencies.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ConsolidatedPlatformForRecruitmentAgencies.DAL
{
    public class RecruitmentContext :DbContext
    {
        public RecruitmentContext():base("RecruitmentDB")
        {

        }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<GraduationYear> GraduationYears { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<UserPhoto> UserPhoto { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<SDPMessage> SDPMessages { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>()
               .Property(f => f.DeadLine)
                .HasColumnType("datetime2");
        }

    }

}