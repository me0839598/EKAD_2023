using Ekad.Voucher.domain.context;
using Ekad.Voucher.domain.Models;
using Ekad.Voucher.logic.Repository.UnitofWork;
using Ekad.Voucher.logic.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekad.Voucher.logic.Service.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProviderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #region Ramy
        public string ApproveTransaction(long id)
        {
            return _unitOfWork.ProviderRepository.ApproveTransaction(id);
        }
        public IEnumerable<TblProviderWallet> GetWalletTransactionList(ProviderWalletFilter filter)
        {
            return _unitOfWork.ProviderRepository.GetWalletTransactionList(filter);
        }
        public IEnumerable<TblProviderService> GetServiceList(ProviderServiceFilter filter)
        {
            return _unitOfWork.ProviderRepository.GetServiceList(filter);
        }

        public TblProviderService? GetServiceById(long ProviderServiceId, bool isfull = false)
        {
            return _unitOfWork.ProviderRepository.GetServiceById(ProviderServiceId, isfull);
        }
        public string UpdateProviderService(TblProviderService entity)
        {
            return _unitOfWork.ProviderRepository.UpdateProviderService(entity);
        }
         public string AddProviderService(TblProviderService entity)
        {
            return _unitOfWork.ProviderRepository.AddProviderService(entity);
        }

        public IEnumerable<TblProviderBranch> GetProviderBranchList(ProviderBranchFilter filter)
        {
            return _unitOfWork.ProviderRepository.GetProviderBranchList(filter);
        }

        public TblProviderBranch? GetProviderBranchById(long ProviderBranchId, bool isfull = false)
        {
            return _unitOfWork.ProviderRepository.GetProviderBranchById(ProviderBranchId, isfull);
        }
        public string UpdateProviderBranch(TblProviderBranch entity)
        {
            return _unitOfWork.ProviderRepository.UpdateProviderBranch(entity);
        }
        public string AddProviderBranch(TblProviderBranch entity)
        {
            return _unitOfWork.ProviderRepository.AddProviderBranch(entity);
        }
        #endregion
        public Task Add(TblProvider entity)
        {
            return _unitOfWork.ProviderRepository.Add(entity);
        }
        public Task AddProvierWallet(TblProviderWallet entity)
        {
            return _unitOfWork.ProviderRepository.AddProvierWallet(entity);
        }
        public string Delete(int id)
        {
            TblProvider entity = _unitOfWork.ProviderRepository.Get(id).Result;
            return _unitOfWork.ProviderRepository.Delete(entity);
        }

        public string Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<TblProvider> Get(int id)
        {
            return _unitOfWork.ProviderRepository.Get(id);
        }

        public Task<TblProvider> Get(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TblProvider> GetAdvertisementType()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TblProvider>> GetAll()
        {
            return _unitOfWork.ProviderRepository.GetAll();
        }

        public Task<IEnumerable<TblProvider>> GetAll(GeneralFilter filter)
        {
            return _unitOfWork.ProviderRepository.GetAll(filter);
        }

        public IEnumerable<TblProviderEmployee> GetEmployeeList(ProviderEmployeeFilter filter)
        {
            return _unitOfWork.ProviderRepository.GetEmployeeList(filter);
        }
        public IEnumerable<TblProviderBranch>GetListProviderBranch(ProviderbrachesFilter filter) {
            return _unitOfWork.ProviderRepository.GetListProviderBranch(filter);
        }
        public Task<IEnumerable<TblCompanyProvider>> GetByCompany(int id)
        {
            return _unitOfWork.ProviderRepository.GetByCompany(id);
        }
        public IEnumerable<TblProvider> SearchProvider(Providerfilter filter)
        {
            return _unitOfWork.ProviderRepository.SearchProvider(filter);
        }
        public string Update(TblProvider entity)
        {
            return _unitOfWork.ProviderRepository.Update(entity);
        }
        public IEnumerable<TblVoucher> GetVoucherList(VoucherFilter filter)
        {
            return _unitOfWork.ProviderRepository.GetVoucherList(filter);
        }

        public TblProviderEmployee? GetEmployeeByUserID(string LoginUserid)
        {
            return _unitOfWork.ProviderRepository.GetEmployeeByUserID(LoginUserid);
        }

        public TblProviderEmployee? GetEmployeeByUserIDFull(string LoginUserid)
        {
            return _unitOfWork.ProviderRepository.GetEmployeeByUserIDFull(LoginUserid);
        }

        public TblVoucher? GetVoucherById(long VoucherId, bool isFull = false)
        {
            return _unitOfWork.ProviderRepository.GetVoucherById(VoucherId, isFull);
        }

        public string UpdateVoucher(TblVoucher entity)
        {
            return _unitOfWork.ProviderRepository.UpdateVoucher(entity);
        }
        public Task SaveCompanyProviders(List<TblCompanyProvider> tableDataList)
        {
            return _unitOfWork.ProviderRepository.SaveCompanyProviders(tableDataList);
        }
        public TblTransaction? GetTransactionByCode(string code, bool IsFull = false)
        {
            return _unitOfWork.ProviderRepository.GetTransactionByCode(code, IsFull);
        }


    }
}
