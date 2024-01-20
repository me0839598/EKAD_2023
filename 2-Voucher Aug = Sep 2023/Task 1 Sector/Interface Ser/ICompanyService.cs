using Ekad.Voucher.domain.context;
using Ekad.Voucher.domain.Models;
using Ekad.Voucher.domain.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekad.Voucher.logic.Service.Interfaces
{
    public interface  ICompanyService : IGenericService<TblCompany>
    {
        TblEmployee? GetEmployeeByUserID(string LoginUserid);
        TblEmployee? GetEmployeeByUserIDFull(string LoginUserid);

        IEnumerable<TblEmployee> GetEmployeeList(EmployeeFilter filter);
        string updateEmployee(TblEmployee entity);
        // Get Id Sector 
        TblSector? GetSectorID(int Sectorid);
        //Update sector
        string Updatesec(TblSector entity);
        //Get sector
        IEnumerable<TblSector> Getsector(sectorfilter filter);
        //ADD sector
        //Task AddSector(TblSector entity);
        Task AddSect(TblSector entity);

        //DeleteSector
        string DeleteSector(int id);

        IEnumerable<TblSector> GetsectorList(GeneralFilter filter);
        IEnumerable<TblCompany> GetCompanies(CompanyFilter filter);
        string ValidateNewTransaction(TransactionVM obj, TblEmployee userobj);

        List<EmpVoucherTotalVM>? GetVoucherForNewTransaction(TransactionVM obj, TblEmployee userobj);

        TblTransaction? AddTransaction(TblTransaction entity);
        TblTransaction? GetTransactionByID(long Id, bool IsFull = false);
        IEnumerable<TblTransaction> GetTransactionList(TransactionFilter filter, bool IsFull = false);
    }
}
