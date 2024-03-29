﻿using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface ISemesterRepository : IGenericRepository<Semester>
    {
        Semester GetSemesterByYearAndSession(int year, string season);
    }
}
