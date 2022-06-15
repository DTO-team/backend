using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.Dtos;
using Models.Response;

namespace CapstoneOnGoing.Services.Interfaces
{
    public interface IApplicationService
    {
        GetApplicationDTO GetApplicationById(Guid id);
    }
}
