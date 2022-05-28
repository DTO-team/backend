﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IApplicationRepository Applications { get; }
        IApplicationStatusRepository Status { get; }
        ICompanyRepository Companies { get; }
        ICouncilLecturerRepository CouncilLecturer { get;}
        ICouncilProjectRepository CouncilProject { get; }
        ICouncilRepository Councils { get; }
        ICriterionRepository Criteria { get; }
        IEvaluationSessionCriterionRepository EvaluationSessionCriterion { get; }
        IEvaluationSessionRepository EvaluationSession { get; }
        IGradeCopyRepository GradeCopy { get; }
        IGradeRepository Grade { get; }
        ILecturerRepository Lecturer { get; }
        IMentorRepository Mentor { get; }
        IProjectRepository Project { get; }
        IQuestionCopyRepository QuestionCopy { get; }
        IQuestionRepository Question { get; }
        IReviewGradeRepository ReviewGrade { get; }
        IReviewQuestionRepository ReviewQuestion { get; }
        IReviewRepository Review { get; }
        IRoleRepository Role { get; }
        ISemesterCriterionRepository SememesterCriterion { get; }
        ISemesterRepository Semester { get; }
        IStudentRepository Student { get; }
        ITeamRepository Team { get; }
        ITeamStudentRepository TeamStudent { get; }
        ITopicLecturerRepository TopicLecturer { get; }
        ITopicRepository Topic { get; }
        IUserRepository User { get; }
        IUserStatusRepository UserStatus { get; }

        public void Save();
    }
}