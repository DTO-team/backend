using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneOnGoing.Services.Interfaces
{
    public interface ICouncilLecturerService
    {
        IEnumerable<CouncilLecturer> GetAllCouncilLecturer();
        CouncilLecturer GetCouncilLecturerById(Guid councilLecturerId);
        void CreateCouncilLecturer(CouncilLecturer councilLecturer);
        void UpdateCouncilLecturer(CouncilLecturer councilLecturer);
        void DeleteCouncilLecturer(CouncilLecturer councilLecturer);
        void DeleteCouncilLecturerById(Guid councilLectuerId);
    }
}
