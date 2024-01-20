using Ekad.Voucher.domain.context;
using Ekad.Voucher.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekad.Voucher.logic.Repository.Interfaces
{
    public interface IProviderRepository : IGenericRepository<TblProvider>
    {
        #region Ramy
        string ApproveTransaction(long id);
        IEnumerable<TblProviderWallet> GetWalletTransactionList(ProviderWalletFilter filter);
        IEnumerable<TblProviderService> GetServiceList(ProviderServiceFilter filter);
        TblProviderService? GetServiceById(long ProviderServiceId, bool isfull = false);
        string UpdateProviderService(TblProviderService entity);
        string AddProviderService(TblProviderService entity);

        IEnumerable<TblProviderBranch> GetProviderBranchList(ProviderBranchFilter filter);
        TblProviderBranch? GetProviderBranchById(long ProviderBranchId, bool isfull = false);
        string UpdateProviderBranch(TblProviderBranch entity);
        string AddProviderBranch(TblProviderBranch entity);
        #endregion
        IEnumerable<TblProviderEmployee> GetEmployeeList(ProviderEmployeeFilter filter);
        IEnumerable<TblProvider> SearchProvider(Providerfilter filter);
        IEnumerable<TblVoucher> GetVoucherList(VoucherFilter filter);
        IEnumerable<TblProviderBranch> GetListProviderBranch(ProviderbrachesFilter filter);
        TblProviderEmployee? GetEmployeeByUserID(string LoginUserid);
        TblProviderEmployee? GetEmployeeByUserIDFull(string LoginUserid);
        TblVoucher? GetVoucherById(long VoucherId, bool isFull = false);
        string UpdateVoucher(TblVoucher entity);
        Task<IEnumerable<TblCompanyProvider>> GetByCompany(int id);
        Task SaveCompanyProviders(List<TblCompanyProvider> tableDataList);
        TblTransaction? GetTransactionByCode(string code, bool IsFull = false);
        Task AddProvierWallet(TblProviderWallet entity);
    }
}
