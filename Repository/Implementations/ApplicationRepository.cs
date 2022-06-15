﻿using Models.Models;
using Repository.Interfaces;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Repository.Implementations
{
    public class ApplicationRepository : GenericRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }

        public Application GetApplicationWithTeamTopicProject(Guid Id)
        {
           Application result = dbSet
                .Include(x => x.Team)
                .Include(x => x.Project)
                .Include(x => x.Topic)
                .AsNoTracking()
                .FirstOrDefault(application => application.Id == Id);
           return result;
        }
    }
}
