using System;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Services.Interfaces;
using Models.Models;
using Repository.Interfaces;

namespace CapstoneOnGoing.Services.Implements
{
    public class TeamStudentService : ITeamStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeamStudentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public TeamStudent GetTeamStudentByStudentId(Guid studentId)
        {
            TeamStudent teamStudent = _unitOfWork.TeamStudent.Get(x => x.StudentId.Equals(studentId)).FirstOrDefault();
            return teamStudent;
        }
    }
}
