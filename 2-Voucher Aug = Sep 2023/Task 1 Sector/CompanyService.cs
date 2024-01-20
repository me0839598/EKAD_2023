using Ekad.Voucher.domain.context;
using Ekad.Voucher.domain.Models;
using Ekad.Voucher.domain.Models.VM;
using Ekad.Voucher.logic.Repository.UnitofWork;
using Ekad.Voucher.logic.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekad.Voucher.logic.Service.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task Add(TblCompany entity)
        {
            return _unitOfWork.CompanyRepository.Add(entity);
        }
        // Add Sector
        //public Task AddSector(TblSector entity)
        //{
        //    return _unitOfWork.CompanyRepository.AddSector(entity);
        //}
       
        public Task AddSect(TblSector entity)
        {
            return _unitOfWork.CompanyRepository.AddSect(entity);
        }


       

    public string Delete(int id)
        {
            TblCompany entity = _unitOfWork.CompanyRepository.Get(id).Result;
            return _unitOfWork.CompanyRepository.Delete(entity);
        }
        
        
        // Delete sector
        public string DeleteSector(int id)
        {
           return _unitOfWork.CompanyRepository.Deletesector(id);
        }



        public string Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<TblCompany> Get(int id)
        {
            return _unitOfWork.CompanyRepository.Get(id);
        }

        public TblSector ? GetSectorID(int id)
        {
            return _unitOfWork.CompanyRepository.GetSectorID(id);
        }
        
        public Task<TblCompany> Get(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TblCompany> GetAdvertisementType()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TblCompany>> GetAll()
        {
            return _unitOfWork.CompanyRepository.GetAll();
        }

        public Task<IEnumerable<TblCompany>> GetAll(GeneralFilter filter)
        {
            return _unitOfWork.CompanyRepository.GetAll(filter);
        }
        public IEnumerable<TblCompany>GetCompanies(CompanyFilter filter)
        {
            return _unitOfWork.CompanyRepository.GetCompanies(filter);
        }

        public TblEmployee? GetEmployeeByUserID(string LoginUserid)
        {
            return _unitOfWork.CompanyRepository.GetEmployeeByUserID(LoginUserid);
        }
        public TblEmployee? GetEmployeeByUserIDFull(string LoginUserid)
        {
            return _unitOfWork.CompanyRepository.GetEmployeeByUserIDFull(LoginUserid);
        }
        public IEnumerable<TblEmployee> GetEmployeeList(EmployeeFilter filter)
        {
            return _unitOfWork.CompanyRepository.GetEmployeeList(filter);
        }

        public IEnumerable<TblSector> GetsectorList(GeneralFilter filter)
        {
            return _unitOfWork.CompanyRepository.GetsectorList(filter);
        }

        //Get sector
        public IEnumerable<TblSector> Getsector(sectorfilter filter)
        {
            return _unitOfWork.CompanyRepository.Getsector(filter);
        }
           //update Sector 
        public string Updatesec(TblSector entity)
        {
            return _unitOfWork.CompanyRepository.Updatesec(entity);
        }
        public string Update(TblCompany entity)
        {
            return _unitOfWork.CompanyRepository.Update(entity);
        }

        public string updateEmployee(TblEmployee entity)
        {
            return _unitOfWork.CompanyRepository.updateEmployee(entity);
        }

        public string ValidateNewTransaction(TransactionVM obj, TblEmployee userobj)
        {
            return _unitOfWork.CompanyRepository.ValidateNewTransaction(obj, userobj);
        }
        public List<EmpVoucherTotalVM>? GetVoucherForNewTransaction(TransactionVM obj, TblEmployee userobj)
        {
            return _unitOfWork.CompanyRepository.GetVoucherForNewTransaction(obj, userobj);
        }

        public TblTransaction? AddTransaction(TblTransaction entity)
        {
            return _unitOfWork.CompanyRepository.AddTransaction(entity);
        }
        public TblTransaction? GetTransactionByID(long Id, bool IsFull = false)
        {
            return _unitOfWork.CompanyRepository.GetTransactionByID(Id, IsFull);
        }
        public IEnumerable<TblTransaction> GetTransactionList(TransactionFilter filter, bool IsFull = false)
        {
            return _unitOfWork.CompanyRepository.GetTransactionList(filter, IsFull);
        }
    }
}
