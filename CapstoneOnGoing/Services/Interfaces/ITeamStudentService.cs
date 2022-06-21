using System;
using Models.Models;

namespace CapstoneOnGoing.Services.Interfaces
{
    public interface ITeamStudentService
    {
        TeamStudent GetTeamStudentByStudentId(Guid studentId);
    }
}
