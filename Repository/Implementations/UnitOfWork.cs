using Repository.Interfaces;
using System;

namespace Repository.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CAPSTONEONGOINGContext _context;
        private readonly IApplicationRepository _applicationRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICouncilLecturerRepository _councilLecturerRepository;
        private readonly ICouncilProjectRepository _councilProjectRepository;
        private readonly ICouncilRepository _councilRepository;
        private readonly ICriterionRepository _criterionRepository;
        private readonly IEvaluationSessionCriterionRepository _evaluationSessionCriterionRepository;
        private readonly IEvaluationSessionRepository _evaluationSessionRepository;
        private readonly IGradeCopyRepository _gradeCopyRepository;
        private readonly IGradeRepository _gradeRepository;
        private readonly ILecturerRepository _lecturerRepository;
        private readonly IMentorRepository _mentorRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IQuestionCopyRepository _questionCopyRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IReviewGradeRepository _reviewGradeRepository;
        private readonly IReviewQuestionRepository _reviewQuestionRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ISemesterCriterionRepository _semesterCriterionRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamStudentRepository _teamStudentRepository;
        private readonly ITopicLecturerRepository _topicLecturerRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IEvidenceRepository _evidenceRepository;
        private readonly IReportRepository _reportRepository;
        private bool _disposed = false;

        public UnitOfWork(CAPSTONEONGOINGContext context,
                          IApplicationRepository applicationRepository, 
                          ICompanyRepository companyRepository, 
                          ICouncilLecturerRepository councilLecturerRepository, 
                          ICouncilProjectRepository councilProjectRepository, 
                          ICouncilRepository councilRepository, 
                          ICriterionRepository criterionRepository, 
                          IEvaluationSessionCriterionRepository evaluationSessionCriterionRepository, 
                          IEvaluationSessionRepository evaluationSessionRepository, 
                          IGradeCopyRepository gradeCopyRepository, 
                          IGradeRepository gradeRepository, 
                          ILecturerRepository lecturerRepository, 
                          IMentorRepository mentorRepository, 
                          IProjectRepository projectRepository, 
                          IQuestionCopyRepository questionCopyRepository, 
                          IQuestionRepository questionRepository, 
                          IReviewGradeRepository reviewGradeRepository, 
                          IReviewQuestionRepository reviewQuestionRepository, 
                          IReviewRepository reviewRepository, 
                          IRoleRepository roleRepository, 
                          ISemesterCriterionRepository semesterCriterionRepository, 
                          ISemesterRepository semesterRepository, 
                          IStudentRepository studentRepository, 
                          ITeamRepository teamRepository, 
                          ITeamStudentRepository teamStudentRepository, 
                          ITopicLecturerRepository topicLecturerRepository, 
                          ITopicRepository topicRepository, 
                          IUserRepository userRepository, 
                          IDepartmentRepository departmentRepository,
                          IEvidenceRepository evidenceRepository,
                          IReportRepository reportRepository)
        {
            _context = context;
            _applicationRepository = applicationRepository;
            _companyRepository = companyRepository;
            _councilLecturerRepository = councilLecturerRepository;
            _councilProjectRepository = councilProjectRepository;
            _councilRepository = councilRepository;
            _criterionRepository = criterionRepository;
            _evaluationSessionCriterionRepository = evaluationSessionCriterionRepository;
            _evaluationSessionRepository = evaluationSessionRepository;
            _gradeCopyRepository = gradeCopyRepository;
            _gradeRepository = gradeRepository;
            _lecturerRepository = lecturerRepository;
            _mentorRepository = mentorRepository;
            _projectRepository = projectRepository;
            _questionCopyRepository = questionCopyRepository;
            _questionRepository = questionRepository;
            _reviewGradeRepository = reviewGradeRepository;
            _reviewQuestionRepository = reviewQuestionRepository;
            _reviewRepository = reviewRepository;
            _roleRepository = roleRepository;
            _semesterCriterionRepository = semesterCriterionRepository;
            _semesterRepository = semesterRepository;
            _studentRepository = studentRepository;
            _teamRepository = teamRepository;
            _teamStudentRepository = teamStudentRepository;
            _topicLecturerRepository = topicLecturerRepository;
            _topicRepository = topicRepository;
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _evidenceRepository = evidenceRepository;
            _reportRepository = reportRepository;
        }

        public IApplicationRepository Applications => _applicationRepository;

        public ICompanyRepository Companies => _companyRepository;

        public ICouncilLecturerRepository CouncilLecturer => _councilLecturerRepository;

        public ICouncilProjectRepository CouncilProject => _councilProjectRepository;

        public ICouncilRepository Councils => _councilRepository;

        public ICriterionRepository Criteria => _criterionRepository;

        public IEvaluationSessionCriterionRepository EvaluationSessionCriterion => _evaluationSessionCriterionRepository;

        public IEvaluationSessionRepository EvaluationSession => _evaluationSessionRepository;

        public IGradeCopyRepository GradeCopy => _gradeCopyRepository;

        public IGradeRepository Grade => _gradeRepository;

        public ILecturerRepository Lecturer => _lecturerRepository;

        public IMentorRepository Mentor => _mentorRepository;

        public IProjectRepository Project => _projectRepository;

        public IQuestionCopyRepository QuestionCopy => _questionCopyRepository;

        public IQuestionRepository Question => _questionRepository;

        public IReviewGradeRepository ReviewGrade => _reviewGradeRepository;

        public IReviewQuestionRepository ReviewQuestion => _reviewQuestionRepository;

        public IReviewRepository Review => _reviewRepository;

        public IRoleRepository Role => _roleRepository;

        public ISemesterCriterionRepository SememesterCriterion => _semesterCriterionRepository;

        public ISemesterRepository Semester => _semesterRepository;

        public IStudentRepository Student => _studentRepository;

        public ITeamRepository Team => _teamRepository;

        public ITeamStudentRepository TeamStudent => _teamStudentRepository;

        public ITopicLecturerRepository TopicLecturer => _topicLecturerRepository;

        public ITopicRepository Topic => _topicRepository;

        public IUserRepository User => _userRepository;

        public IDepartmentRepository Department => _departmentRepository;

        public IEvidenceRepository Evidence => _evidenceRepository;

        public IReportRepository Report => _reportRepository;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public int Save()
        {
           return _context.SaveChanges();
        }
    }
}
