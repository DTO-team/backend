using CapstoneOnGoing.Services.Interfaces;
using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;

namespace CapstoneOnGoing.Services.Implements
{
    public class CompanyService : IComanyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //Create company
        public void CreateCompany(Company company)
        {
            _unitOfWork.Companies.Insert(company);
        }

        //Delete company
        public void DeleteCompany(Company companyToDelete)
        {
            _unitOfWork.Companies.Delete(companyToDelete);
        }

        //Delete company by id
        public void DeleteCompanyById(Guid companyId)
        {
            _unitOfWork.Companies.DeleteById(companyId);
        }

        //Get company list
        public IEnumerable<Company> GetAllCompanies()
        {
            IEnumerable<Company> companies = _unitOfWork.Companies.Get();
            return companies;
        }

        //Get company by id
        public Company GetCompanyById(Guid companyId)
        {
            Company company = _unitOfWork.Companies.GetById(companyId);
            return company;
        }

        //Update company
        public void UpdateCompany(Company companyToUpdate)
        {
            _unitOfWork.Companies.Update(companyToUpdate);
        }
    }
}
