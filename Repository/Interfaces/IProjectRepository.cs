﻿using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        IEnumerable<Project> GetAllProjectWithMentorTeamAndTeamStudents(string searchString);
    }
}
