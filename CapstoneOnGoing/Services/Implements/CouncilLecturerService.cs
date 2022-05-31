using CapstoneOnGoing.Services.Interfaces;
using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneOnGoing.Services.Implements
{
    public class CouncilLecturerService : ICouncilLecturerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CouncilLecturerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreateCouncilLecturer(CouncilLecturer councilLecturer)
        {
            _unitOfWork.CouncilLecturer.Insert(councilLecturer);
        }

        public void DeleteCouncilLecturer(CouncilLecturer councilLecturer)
        {
            _unitOfWork.CouncilLecturer.Delete(councilLecturer);
        }

        public void DeleteCouncilLecturerById(Guid councilLectuerId)
        {
            _unitOfWork.CouncilLecturer.DeleteById(councilLectuerId);
        }

        public IEnumerable<CouncilLecturer> GetAllCouncilLecturer()
        {
            IEnumerable<CouncilLecturer> councilLecturers = _unitOfWork.CouncilLecturer.Get();
            return councilLecturers;
        }

        public CouncilLecturer GetCouncilLecturerById(Guid councilLecturerId)
        {
            CouncilLecturer councilLecturer = _unitOfWork.CouncilLecturer.GetById(councilLecturerId);
            return councilLecturer;
        }

        public void UpdateCouncilLecturer(CouncilLecturer councilLecturer)
        {
            _unitOfWork.CouncilLecturer.Update(councilLecturer);
        }
    }
}
