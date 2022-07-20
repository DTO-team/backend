using CapstoneOnGoing.Services.Implements;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
			services.AddScoped<IReportRepository, ReportRepository>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IStudentService, StudentService>();
			services.AddScoped<ILecturerService, LecturerService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<ISemesterService, SemesterService>();
			services.AddScoped<ITopicService, TopicService>();
			services.AddScoped<ITeamService, TeamService>();
			services.AddScoped<IApplicationService, ApplicationService>();
            services.AddScoped<ITeamStudentService, TeamStudentService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IWeekRepository, WeekRepository>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<ICriterionRepository, CriterionRepository>();
            services.AddScoped<ICriterionService, CriterionService>();
            services.AddScoped<ICouncilService, CouncilService>();
            services.AddScoped<IEvaluationSessionService, EvaluationSessionService>();
            services.AddScoped<IEvaluationReportRepository, EvaluationReportRepository>();
            services.AddScoped<IEvaluationReportDetailRepository, EvaluationReportDetailRepository>();
        }

		public static void AddConfigUriForPagination(this IServiceCollection services)
		{
			services.AddHttpContextAccessor();
			services.AddSingleton<IUriService>(impl =>
			{
				var accessor = impl.GetRequiredService<IHttpContextAccessor>();
				var request = accessor.HttpContext.Request;
				var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
				return new UriService(uri);
			});
		}

		public static void AddRedisService(this IServiceCollection services)
		{
			services.AddStackExchangeRedisCache(options =>
			{
				options.Configuration =
					$"{Startup.Configuration.GetValue<string>("REDIS_HOST")}:{Startup.Configuration.GetValue<string>("REDIS_PORT")}";
			});
		}
	}
}
