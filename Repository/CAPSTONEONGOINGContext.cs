using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Models.Models;

#nullable disable

namespace Repository
{
    public partial class CAPSTONEONGOINGContext : DbContext
    {
        public CAPSTONEONGOINGContext()
        {
        }

        public CAPSTONEONGOINGContext(DbContextOptions<CAPSTONEONGOINGContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Council> Councils { get; set; }
        public virtual DbSet<CouncilLecturer> CouncilLecturers { get; set; }
        public virtual DbSet<CouncilProject> CouncilProjects { get; set; }
        public virtual DbSet<Criterion> Criteria { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<EvaluationSession> EvaluationSessions { get; set; }
        public virtual DbSet<EvaluationSessionCriterion> EvaluationSessionCriteria { get; set; }
        public virtual DbSet<Evidence> Evidences { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<GradeCopy> GradeCopies { get; set; }
        public virtual DbSet<Lecturer> Lecturers { get; set; }
        public virtual DbSet<Mentor> Mentors { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionCopy> QuestionCopies { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<ReviewGrade> ReviewGrades { get; set; }
        public virtual DbSet<ReviewQuestion> ReviewQuestions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Semester> Semesters { get; set; }
        public virtual DbSet<SemesterCriterion> SemesterCriteria { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<TeamStudent> TeamStudents { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<TopicLecturer> TopicLecturers { get; set; }
        public virtual DbSet<User> Users { get; set; }

   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("Application");

                entity.HasIndex(e => new { e.TeamId, e.TopicId }, "AK_Application_TeamId_TopicId")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.StatusId).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Application_TeamID");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.TopicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Application_TopicID");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Company)
                    .HasForeignKey<Company>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Company_UserID");
            });

            modelBuilder.Entity<Council>(entity =>
            {
                entity.ToTable("Council");

                entity.HasIndex(e => e.Id, "Council_Id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.EvaluationSession)
                    .WithMany(p => p.Councils)
                    .HasForeignKey(d => d.EvaluationSessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Council_EvaluationSessionID");
            });

            modelBuilder.Entity<CouncilLecturer>(entity =>
            {
                entity.ToTable("CouncilLecturer");

                entity.HasIndex(e => new { e.CouncilId, e.LecturerId }, "AK_CouncilLecturer_CouncilId_LecturerId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Council)
                    .WithMany(p => p.CouncilLecturers)
                    .HasForeignKey(d => d.CouncilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CouncilLecturer_CouncilID");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.CouncilLecturers)
                    .HasForeignKey(d => d.LecturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CouncilLecturer_LecturerID");
            });

            modelBuilder.Entity<CouncilProject>(entity =>
            {
                entity.ToTable("CouncilProject");

                entity.HasIndex(e => new { e.CouncilId, e.ProjectId }, "AK_CouncilProject_CouncilId_ProjectId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Council)
                    .WithMany(p => p.CouncilProjects)
                    .HasForeignKey(d => d.CouncilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CouncilProject_CouncilID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.CouncilProjects)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CouncilProject_ProjectID");
            });

            modelBuilder.Entity<Criterion>(entity =>
            {
                entity.HasIndex(e => e.Code, "Criteria_Code_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Evaluation)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.HasIndex(e => e.Code, "Department_Code_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.Name, "Department_Name_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<EvaluationSession>(entity =>
            {
                entity.ToTable("EvaluationSession");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.EvaluationSessions)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EvaluationSession_SemesterID");
            });

            modelBuilder.Entity<EvaluationSessionCriterion>(entity =>
            {
                entity.HasIndex(e => new { e.EvaluationSessionId, e.CriteriaId }, "AK_EvaluationSessionCriteria_EvaluationSessionId_CriteriaId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Criteria)
                    .WithMany(p => p.EvaluationSessionCriteria)
                    .HasForeignKey(d => d.CriteriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EvaluationSessionCriteria_CriteriaCriteriaCopyID");

                entity.HasOne(d => d.EvaluationSession)
                    .WithMany(p => p.EvaluationSessionCriteria)
                    .HasForeignKey(d => d.EvaluationSessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EvaluationSessionCriteria_EvaluationSessionID");
            });

            modelBuilder.Entity<Evidence>(entity =>
            {
                entity.ToTable("Evidence");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.Evidences)
                    .HasForeignKey(d => d.ReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Evidence_ReportID");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.ToTable("Grade");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Criteria)
                    .WithMany(p => p.Grades)
                    .HasForeignKey(d => d.CriteriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Grade_CriteriaID");
            });

            modelBuilder.Entity<GradeCopy>(entity =>
            {
                entity.ToTable("GradeCopy");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.Criteria)
                    .WithMany(p => p.GradeCopies)
                    .HasForeignKey(d => d.CriteriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GradeCopy_CriteriaCopyID");
            });

            modelBuilder.Entity<Lecturer>(entity =>
            {
                entity.ToTable("Lecturer");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Lecturers)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lecturer_DepartmentID");

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Lecturer)
                    .HasForeignKey<Lecturer>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lecturer_UserID");
            });

            modelBuilder.Entity<Mentor>(entity =>
            {
                entity.ToTable("Mentor");

                entity.HasIndex(e => new { e.ProjectId, e.LecturerId }, "AK_Mentor_TeamId_LecturerId")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.Mentors)
                    .HasForeignKey(d => d.LecturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mentor_LecturerID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Mentors)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mentor_ProjectID");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");

                entity.HasIndex(e => e.ApplicationId, "Project_ApplicationId_uindex")
                    .IsUnique();

                entity.HasIndex(e => new { e.TeamId, e.ApplicationId }, "Project_TeamId_ApplicationId_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.TeamId, "Project_TeamId_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Application)
                    .WithOne(p => p.Project)
                    .HasForeignKey<Project>(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_ApplicationID");

                entity.HasOne(d => d.Team)
                    .WithOne(p => p.Project)
                    .HasForeignKey<Project>(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Project_TeamID");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Priority)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SubCriteria)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Criteria)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.CriteriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Question_CriteriaID");
            });

            modelBuilder.Entity<QuestionCopy>(entity =>
            {
                entity.ToTable("QuestionCopy");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Priority)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Criteria)
                    .WithMany(p => p.QuestionCopies)
                    .HasForeignKey(d => d.CriteriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionCopy_CriteriaCopyID");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Report");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CompletedTasks).HasMaxLength(3000);

                entity.Property(e => e.Feedbacks).HasMaxLength(3000);

                entity.Property(e => e.InProgressTasks).HasMaxLength(3000);

                entity.Property(e => e.NextWeekTasks).HasMaxLength(3000);

                entity.Property(e => e.SelfAssessments).HasMaxLength(3000);

                entity.Property(e => e.UrgentIssues).HasMaxLength(3000);

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Report_ProjectID");

                entity.HasOne(d => d.Reporter)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.ReporterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Report_StudentID");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Review");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Council)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.CouncilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Review_CouncilID");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.LecturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Review_LecturerID");
            });

            modelBuilder.Entity<ReviewGrade>(entity =>
            {
                entity.ToTable("ReviewGrade");

                entity.HasIndex(e => new { e.ReviewId, e.GradeId }, "AK_ReviewGradeCopy_ReviewId_GradeId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Comment).HasMaxLength(1000);

                entity.HasOne(d => d.Grade)
                    .WithMany(p => p.ReviewGrades)
                    .HasForeignKey(d => d.GradeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReviewGradeCopy_GradeCopyID");

                entity.HasOne(d => d.Review)
                    .WithMany(p => p.ReviewGrades)
                    .HasForeignKey(d => d.ReviewId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReviewGradeCopy_ReviewID");
            });

            modelBuilder.Entity<ReviewQuestion>(entity =>
            {
                entity.ToTable("ReviewQuestion");

                entity.HasIndex(e => new { e.ReviewId, e.QuestionId }, "AK_ReviewQuestion_ReviewId_QuestionId")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Answer)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Note).HasMaxLength(1000);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.ReviewQuestions)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReviewQuestion_QuestionID");

                entity.HasOne(d => d.Review)
                    .WithMany(p => p.ReviewQuestions)
                    .HasForeignKey(d => d.ReviewId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReviewQuestion_ReviewID");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.HasIndex(e => e.Name, "Role_Name_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.ToTable("Semester");

                entity.HasIndex(e => new { e.Year, e.Season }, "AK_Semester_Year_Season")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Season)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SemesterCriterion>(entity =>
            {
                entity.HasIndex(e => e.Code, "CriteriaCopy_Code_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.SemesterId, "CriteriaCopy_SemesterId_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Evaluation)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Semester)
                    .WithOne(p => p.SemesterCriterion)
                    .HasForeignKey<SemesterCriterion>(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CriteriaCopy_SemesterID");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.HasIndex(e => e.Code, "Student_Code_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Student)
                    .HasForeignKey<Student>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Student_UserID");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK_Student_SemesterID");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Team");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<TeamStudent>(entity =>
            {
                entity.ToTable("TeamStudent");

                entity.HasIndex(e => new { e.TeamId, e.StudentId }, "AK_TeamStudent_TeamId_StudentId")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.TeamStudents)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamStudent_StudentID");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.TeamStudents)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamStudent_TeamID");
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.ToTable("Topic");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Topics)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Topic_CompanyID");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Topics)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Topic_SemesterID");
            });

            modelBuilder.Entity<TopicLecturer>(entity =>
            {
                entity.ToTable("TopicLecturer");

                entity.HasIndex(e => new { e.TopicId, e.LecturerId }, "AK_TopicLecturer_TopicId_LecturerId")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.TopicLecturers)
                    .HasForeignKey(d => d.LecturerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TopicLecturer_LecturerID");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.TopicLecturers)
                    .HasForeignKey(d => d.TopicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TopicLecturer_TopicID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "User_Email_uindex")
                    .IsUnique();

                entity.HasIndex(e => e.UserName, "User_UserName_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .HasMaxLength(74)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.StatusId).HasDefaultValueSql("((1))");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_RoleID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
