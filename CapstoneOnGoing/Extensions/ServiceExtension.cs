using CapstoneOnGoing.Services.Implements;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Repository.Implementations;
using Repository.Interfaces;

namespace CapstoneOnGoing.Extensions
{
    public static class ServiceExtension
    {
        public static void AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICouncilLecturerRepository, CouncilLecturerRepository>();
            services.AddScoped<ICouncilProjectRepository, CouncilProjectRepository>();
            services.AddScoped<ICouncilRepository, CouncilRepository>();
            services.AddScoped<ICriterionRepository, CriterionRepository>();
            services.AddScoped<IEvaluationSessionCriterionRepository, EvaluationSessionCriterionRepository>();
            services.AddScoped<IEvaluationSessionRepository, EvaluationSessionRepository>();
            services.AddScoped<IGradeCopyRepository, GradeCopyRepository>();
            services.AddScoped<IGradeRepository, GradeRepository>();
            services.AddScoped<ILecturerRepository, LecturerRepository>();
            services.AddScoped<IMentorRepository, MentorRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IQuestionCopyRepository, QuestionCopyRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IReviewGradeRepository, ReviewGradeRepository>();
            services.AddScoped<IReviewQuestionRepository, ReviewQuestionRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISemesterCriterionRepository, SemesterCriterionRepository>();
            services.AddScoped<ISemesterRepository, SemesterRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<ITeamStudentRepository, TeamStudentRepository>();
            services.AddScoped<ITopicLecturerRepository, TopicLecturerRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IEvidenceRepository, EvidenceRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ILecturerService, LecturerService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISemesterService, SemesterService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<IApplicationService, ApplicationService>();
        }
    }
}
