using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneOnGoing.Services.Interfaces
{
    public interface IComanyService
    {
        IEnumerable<Company> GetAllCompanies();
        Company GetCompanyById(Guid companyId);
        void CreateCompany(Company company);
        void UpdateCompany(Company companyToUpdate);
        void DeleteCompany(Company companyToDelete);
        void DeleteCompanyById(Guid companyId);
    }
}
