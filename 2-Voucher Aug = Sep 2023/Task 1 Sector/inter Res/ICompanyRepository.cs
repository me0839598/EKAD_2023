using Ekad.Voucher.domain.context;
using Ekad.Voucher.domain.Models;
using Ekad.Voucher.domain.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekad.Voucher.logic.Repository.Interfaces
{
    public interface ICompanyRepository : IGenericRepository<TblCompany>
    {
        IEnumerable<TblEmployee> GetEmployeeList(EmployeeFilter filter);
        TblEmployee? GetEmployeeByUserID(string LoginUserid);
        TblEmployee? GetEmployeeByUserIDFull(string LoginUserid);
        // Get Id Sector 
        TblSector? GetSectorID(int Sectorid);
        string updateEmployee(TblEmployee entity);
        string Updatesec(TblSector entity);
        IEnumerable<TblSector> GetsectorList(GeneralFilter filter);
            // setor
        IEnumerable<TblSector> Getsector(sectorfilter filter);

        //Delete Sector
        string Deletesector(int id);

        //ADD sector
        Task AddSect(TblSector entity);
        //Task AddSector(TblSector entity);

        IEnumerable<TblCompany> GetCompanies(CompanyFilter filter);
        string ValidateNewTransaction(TransactionVM obj, TblEmployee userobj);
        List<EmpVoucherTotalVM>? GetVoucherForNewTransaction(TransactionVM obj, TblEmployee userobj);

        TblTransaction? AddTransaction(TblTransaction entity);
        TblTransaction? GetTransactionByID(long Id, bool IsFull = false);
        IEnumerable<TblTransaction> GetTransactionList(TransactionFilter filter, bool IsFull = false);

    }
}
