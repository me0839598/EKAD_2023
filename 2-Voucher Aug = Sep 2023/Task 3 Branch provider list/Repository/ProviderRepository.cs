using Amazon.DeviceFarm.Model;
using Ekad.Voucher.domain.context;
using Ekad.Voucher.domain.Models;
using Ekad.Voucher.logic.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Ekad.Voucher.domain.Models.VM;
using Ekad.Voucher.domain.Resources;

namespace Ekad.Voucher.logic.Repository.Repositories
{
    public class ProviderRepository : IProviderRepository
    {
        protected readonly EkadVoucherDbContext _context;
        public ProviderRepository(EkadVoucherDbContext Context)
        {
            _context = Context;
        }

        #region Ramy
        public string ApproveTransaction(long id)
        {
            try
            {
                var transaction=_context.TblTransactions.FirstOrDefault(a=>a.TransactionId==id);
                if (transaction ==null || !transaction.TransactionVerficationCodeEnd.HasValue || transaction.TransactionVerficationCodeEnd.Value<DateTime.Now)
                {
                    return Resource.thisvoucherisExpired;
                }
                decimal PayWithWalletValue = transaction.TblTransactionPayments.FirstOrDefault(a => a.PaymentTypeId == (int)TransactionPaymentTypeEnum.Voucher).PaymentAnount;

                var voucherlist = _context.TblVouchers.Where(a => a.ProviderFk == transaction.ProviderFk && a.IsVoucherValid &&
                a.TblVoucherCompanyRepEmps.Any(x => x.EmpFk == transaction.EmployeeFk)).Select(a => a.VoucherId).ToList();
                if (!voucherlist.Any())
                {
                    return Resource.ThereIsNoValidVouchers;
                }
                var TblVoucherCompanyRepEmp = _context.TblVoucherCompanyRepEmps
                    .Where(a => a.EmpFk == transaction.EmployeeFk && voucherlist.Contains(a.VoucherFk));

                decimal totalincomebalance = TblVoucherCompanyRepEmp == null ? 0 :
                    TblVoucherCompanyRepEmp.Sum(a => a.Amount * a.Factor);
                if (totalincomebalance <= 0)
                {
                    return Resource.YourVoucherBalanceLessThanTheWalletPaymentValue;
                }
                decimal totalOutcomebalance = _context.TblTransactionVouchers.Where(x => x.VoucherFk.HasValue && voucherlist.Contains(x.VoucherFk.Value)
                && x.TransactionFkNavigation.EmployeeFk == transaction.EmployeeFk && x.TransactionFkNavigation.IsActive)
                    .Sum(a => a.VoucherAnount);


                decimal availableBalance = totalincomebalance - totalOutcomebalance;
                if (availableBalance < (PayWithWalletValue))
                {
                    return Resource.YourVoucherBalanceLessThanTheWalletPaymentValue;
                }
                transaction.IsActive=true;
                _context.Entry(transaction).State = EntityState.Modified;
                _context.SaveChanges();

                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }

        public IEnumerable<TblProviderWallet> GetWalletTransactionList(ProviderWalletFilter filter)
        {
            return _context.TblProviderWallets.Where(a => (a.IsActive.HasValue && a.IsActive.Value)
                && (filter.ProviderWalletId == null || filter.ProviderWalletId == 0 || a.ProviderWalletId == filter.ProviderWalletId)
                && (filter.ProviderFk == null || filter.ProviderFk == 0 || a.ProviderFk == filter.ProviderFk)
                && (filter.Factor == null || filter.Factor == 0 || a.Factor == filter.Factor)

                && (filter.TransactionType == null || filter.TransactionType == 0 || a.TransactionType == filter.TransactionType)
                && (filter.PaymentType == null || filter.PaymentType == 0 || a.PaymentType == filter.PaymentType)
                && (filter.TransactionFk == null || filter.TransactionFk == 0 || a.TransactionFk == filter.TransactionFk)
                && (string.IsNullOrEmpty(filter.WaletTransactionNo) || a.WaletTransactionNo == filter.WaletTransactionNo)
                && (!filter.TransactionDateFrom.HasValue || a.TransactionDate.Date >= filter.TransactionDateFrom.Value.Date)
                && (!filter.TransactionDateTo.HasValue || a.TransactionDate.Date <= filter.TransactionDateTo.Value.Date)
                )
                .Include(a => a.TransactionFkNavigation)
                .Include(a => a.PaymentTypeNavigation)
                .Include(a => a.TransactionTypeNavigation)
                .Include(a => a.ProviderFkNavigation);
        }

        public IEnumerable<TblProviderService> GetServiceList(ProviderServiceFilter filter)
        {
            return _context.TblProviderServices.Where(a => 
                (filter.ProviderServiceId == null || filter.ProviderServiceId == 0 || a.ProviderServiceId == filter.ProviderServiceId)
                && (filter.ProviderFk == null || filter.ProviderFk == 0 || a.ProviderFk == filter.ProviderFk)
                && (filter.ServiceFk == null || filter.ServiceFk == 0 || a.ServiceFk == filter.ServiceFk)

                && (filter.IsActive == null  || a.IsActive == filter.IsActive)
                )
                .Include(a => a.ServiceFkNavigation).ThenInclude(a=>a.ServiceTypeFkNavigation)
                .Include(a => a.ProviderFkNavigation);
        }

        public TblProviderService? GetServiceById(long ProviderServiceId,bool isfull=false)
        {
            if (isfull)
            {
                return _context.TblProviderServices.Where(a => a.ProviderServiceId == ProviderServiceId)
                    .Include(a => a.ServiceFkNavigation).ThenInclude(a => a.ServiceTypeFkNavigation)
                    .Include(a => a.ProviderFkNavigation).FirstOrDefault();
            }
            else
            {
                return _context.TblProviderServices.Where(a => a.ProviderServiceId == ProviderServiceId).FirstOrDefault();
            }
        }
        public string UpdateProviderService(TblProviderService entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }

        public string AddProviderService(TblProviderService entity)
        {
            try
            {
                _context.TblProviderServices.Add(entity);
                _context.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }

        public IEnumerable<TblProviderBranch> GetProviderBranchList(ProviderBranchFilter filter)
        {
            return _context.TblProviderBranches.Where(a => !a.IsDeleted &&
                (filter.BranchId == null || filter.BranchId == 0 || a.BranchId == filter.BranchId)
                && (filter.ProviderFk == null || filter.ProviderFk == 0 || a.ProviderFk == filter.ProviderFk)
                && (filter.IsActive == null || a.IsActive == filter.IsActive)

                && (filter.IsActive == null || a.IsActive == filter.IsActive)
                )
                .Include(a => a.CityFkNavigation)
                .Include(a => a.CountryFkNavigation)
                .Include(a => a.ProviderFkNavigation);
        }

        public TblProviderBranch? GetProviderBranchById(long ProviderBranchId, bool isfull = false)
        {
            if (isfull)
            {
                return _context.TblProviderBranches.Where(a => !a.IsDeleted && a.BranchId == ProviderBranchId)
                    .Include(a => a.CityFkNavigation)
                    .Include(a => a.CountryFkNavigation)
                    .Include(a => a.ProviderFkNavigation).FirstOrDefault();
            }
            else
            {
                return _context.TblProviderBranches.Where(a => a.BranchId == ProviderBranchId).FirstOrDefault();
            }
        }
        public string UpdateProviderBranch(TblProviderBranch entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }

        public string AddProviderBranch(TblProviderBranch entity)
        {
            try
            {
                _context.TblProviderBranches.Add(entity);
                _context.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }
        #endregion

        public async System.Threading.Tasks.Task Add(TblProvider entity)
        {
            await _context.TblProviders.AddAsync(entity);
            _context.SaveChanges();
        }
        public async System.Threading.Tasks.Task AddProvierWallet(TblProviderWallet entity)
        {
            await _context.TblProviderWallets.AddAsync(entity);
        }
        //public string RemoveCompanyProviders(int id)
        //{

        //    using(var scope=new TransactionScope())
        //    {
        //        try
        //        {
        //            var CompanyProviders = GetByCompany(id).Result;
        //            foreach (var CompanyProvider in CompanyProviders)
        //            {
        //                _context.TblCompanyProviders.Remove(CompanyProvider);
        //            }
        //            _context.SaveChanges();
        //            scope.Complete();
        //            return "success";

        //        }             
        //        catch(Exception ex)
        //        {
        //            scope.Dispose(); 
        //            throw;
        //        }
        //    }
        //}
        public async System.Threading.Tasks.Task SaveCompanyProviders(List<TblCompanyProvider> tableDataList)
        {
            try
            {
                // saving Company Id in CmpId
                int CmpId = tableDataList[0].CompanyFk;
                //List of providers Id
                List<int> ProviderList = tableDataList.Select(a => a.ProviderFk).ToList();
                //bool to check if any updates happened 
                bool updated=false;               
                using(var scope = new TransactionScope())
                {
                    try
                    {
                        //DeletedList to save the rows to be delete
                        var DeletedList=_context.TblCompanyProviders.Where(a=>a.CompanyFk==CmpId && (!tableDataList.Any() || !ProviderList.Contains(a.ProviderFk))).ToList();
                        //checking if tableDatalist is not empty
                        if (tableDataList[0].ProviderFk != 0)
                        {
                            foreach (var companyProvider in tableDataList)
                            {
                                if (!_context.TblCompanyProviders.Any(a => a.ProviderFk == companyProvider.ProviderFk && a.CompanyFk == companyProvider.CompanyFk))
                                {
                                    await _context.TblCompanyProviders.AddAsync(companyProvider);
                                    updated = true;
                                }

                            }
                        }
                        
                        if (DeletedList.Count > 0)
                        {
                            foreach (var deleted in DeletedList)
                            {
                                _context.TblCompanyProviders.Remove(deleted);
                                updated = true;
                            }
                        }
                        if (updated == true)
                        {
                            _context.SaveChanges();
                        }
                        
                        scope.Complete();
                    }
                    catch(Exception ex)
                    {
                        scope.Dispose();
                        throw;
                    }
                    
                }                
                
            }
            catch(Exception ex)
            {
                
            }
        }

        public string Delete(TblProvider entity)
        {
            try
            {
                _context.TblProviders.Remove(entity);
                _context.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }
   
        public async Task<TblProvider> Get(int id)
        {
            return await _context.TblProviders.Where(a => a.ProviderId == id )
                .Include(a => a.TblTransactionReviews)
            .Include(a => a.TblProviderServiceTypes).ThenInclude(a => a.ServiceTypeFkNavigation)
            .Include(a => a.TblProviderServices).ThenInclude(a => a.ServiceFkNavigation)
                .FirstOrDefaultAsync();
        }


        public Task<TblProvider> Get(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TblProvider>> GetAll()
        {
            return await _context.TblProviders.Where(a => a.IsDeleted != true).ToListAsync();
        }

        public async Task<IEnumerable<TblProvider>> GetAll(GeneralFilter filter)
        {
            return await _context.TblProviders.Where(a => a.IsDeleted != true
            && (filter.id==null ||a.ProviderId==filter.id)
             && (string.IsNullOrEmpty(filter.textLike) || a.ProviderNameAr.Contains(filter.textLike) || a.ProviderNameEn.Contains(filter.textLike))
            ).ToListAsync();
        }
        public async Task<IEnumerable<TblCompanyProvider>> GetByCompany(int id)
        {
            return await _context.TblCompanyProviders.Where(a => a.CompanyFk == id).Include(a => a.CompanyFkNavigation).Include(a=>a.ProviderFkNavigation).Include(a=>a.ProviderFkNavigation.CommessionTypeNavigation).ToListAsync();
        }

        public IEnumerable<TblProvider> SearchProvider(Providerfilter filter)
        {
            filter.PageNum = !filter.PageNum.HasValue || filter.PageNum == 0 ? 1 : filter.PageNum;
            filter.RowNum = !filter.RowNum.HasValue || filter.RowNum == 0 ? 1000000 : filter.RowNum;
            if (string.IsNullOrEmpty(filter.sortBy))
            {
                filter.sortBy = "ProviderId";
            }
            if (string.IsNullOrEmpty(filter.sortDir))
            {
                filter.sortDir = "asc";
            }
            return  _context.TblProviders.Where(a => a.IsDeleted != true
            && (filter.ProviderId == null || a.ProviderId == filter.ProviderId)
            && (filter.CommessionType == null || a.CommessionType == filter.CommessionType)
            && (
                string.IsNullOrEmpty(filter.textLike) || a.ProviderNameAr.Contains(filter.textLike) || a.ProviderNameEn.Contains(filter.textLike)
                || a.ProviderMobile.Contains(filter.textLike) || a.ProviderPhone.Contains(filter.textLike) || a.ProviderEmail.Contains(filter.textLike)
                )
            && (filter.serviceid == null || filter.serviceid==0 || a.TblProviderServiceTypes.Any(X=>X.ServiceTypeFk == filter.serviceid))
             && ((!filter.Longitude.HasValue && !filter.Latitude.HasValue ) || a.TblProviderBranches.Any(m=> 
             !string.IsNullOrEmpty(m.BranchLang) && !string.IsNullOrEmpty(m.BranchLat) &&
             CalculateDistance(Convert.ToDouble(m.BranchLat), Convert.ToDouble(m.BranchLang),
                filter.Latitude.Value, filter.Longitude.Value
               ) <= filter.ProvideServiceInKm) )
            ).OrderBy(filter.sortBy + " " + filter.sortDir)
            .Skip((filter.PageNum.Value - 1) * filter.RowNum.Value).Take(filter.RowNum.Value)
            .Include(a=>a.TblTransactionReviews)
            .Include(a=>a.TblProviderServiceTypes).ThenInclude(a=>a.ServiceTypeFkNavigation).Include(a=>a.CommessionTypeNavigation);
        }
        //ProviderbrachesFilter
        public IEnumerable<TblProviderBranch> GetListProviderBranch(ProviderbrachesFilter filter)
        {
            return _context.TblProviderBranches.Where(a => a.IsDeleted != true
                && (filter.BranchId == null || a.BranchId == filter.BranchId)
                 && (filter.ProviderFk == null || a.ProviderFk == filter.ProviderFk)
                && (string.IsNullOrEmpty(filter.branchname) || a.BranchNameAr.Contains(filter.branchname) || a.BranchNameEn.Contains(filter.branchname))
                && (filter.city == null || a.CityFk == filter.city)
                && (filter.country == null || a.CountryFk == filter.country)
                ).Include(a => a.ProviderFkNavigation)
                .Include(a => a.CountryFkNavigation)
                .Include(a => a.CityFkNavigation);
        }




        public IEnumerable<TblProviderEmployee> GetEmployeeList(ProviderEmployeeFilter filter)
        {
            return _context.TblProviderEmployees.Where(a => a.IsDeleted != true
                && (filter.EmployeeId == null || a.EmployeeId == filter.EmployeeId)
                && (filter.IsActive == null || a.IsActive == filter.IsActive)
                && (filter.IsAdmin == null || a.IsAdmin == filter.IsAdmin)
                && (filter.ProviderFk == null || a.ProviderFk == filter.ProviderFk)
                && (filter.IsAccountant == null || a.IsAccountant == filter.IsAccountant)
                && (string.IsNullOrEmpty(filter.LoginUserid) || a.LoginUserid == filter.LoginUserid)
                && (string.IsNullOrEmpty(filter.LoginBy) || a.EmployeeEmail == filter.LoginBy || a.EmployeeMobile == filter.LoginBy)
                && (
                string.IsNullOrEmpty(filter.textLike) || a.EmployeeNameAr.Contains(filter.textLike) || a.EmployeeNameEn.Contains(filter.textLike)
                || a.EmployeeMobile.Contains(filter.textLike)  || a.EmployeeEmail.Contains(filter.textLike)
                )
                );

        }

        public string Update(TblProvider entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }

        public double CalculateDistance(double sLatitude, double sLongitude, double eLatitude, double eLongitude)
        {
            Location point1 = new Location() { Latitude = sLatitude, Longitude = sLongitude };
            Location point2 = new Location() { Latitude = eLatitude, Longitude = eLongitude };
            var d1 = point1.Latitude * (Math.PI / 180.0);
            var num1 = point1.Longitude * (Math.PI / 180.0);
            var d2 = point2.Latitude * (Math.PI / 180.0);
            var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3))) / 1000;
        }

        public IEnumerable<TblVoucher> GetVoucherList(VoucherFilter filter)
        {
            return _context.TblVouchers.Where(a => a.IsDeleted != true
                && (filter.VoucherId == null || filter.VoucherId == 0 || a.VoucherId == filter.VoucherId)
                && (filter.CompanyFk == null || filter.CompanyFk == 0 || a.CompanyFk == filter.CompanyFk)
                && (filter.SectorFk == null || filter.SectorFk == 0 || a.SectorFk == filter.SectorFk)

                && (filter.ProviderFk == null || filter.ProviderFk == 0 || a.ProviderFk == filter.ProviderFk)
                && (string.IsNullOrEmpty(filter.VoucherCode) || a.VoucherCode == filter.VoucherCode)
                && (string.IsNullOrEmpty(filter.VoucherNo) || a.VoucherNo == filter.VoucherNo )
                && (filter.ApprovedByProvider == null || a.ApprovedByProvider == filter.ApprovedByProvider)
                && (filter.ApprovedBySector == null || a.ApprovedBySector == filter.ApprovedBySector)
                && (filter.NeedActionFromProvider == null || a.NeedActionFromProvider == filter.NeedActionFromProvider)
                && (filter.NeedActionFromSector == null || a.NeedActionFromSector == filter.NeedActionFromSector)
                && (filter.IsVoucherValid == null || a.IsVoucherValid == filter.IsVoucherValid)
                )
                .Include(a=>a.SectorFkNavigation)
                .Include(a=>a.ProviderFkNavigation)
                .Include(a => a.CompanyFkNavigation);
        }

        public IEnumerable<TblVoucher> GetVoucherListWithTransactions(VoucherFilter filter)
        {
            return _context.TblVouchers.Where(a => a.IsDeleted != true
                && (filter.VoucherId == null || filter.VoucherId == 0 || a.VoucherId == filter.VoucherId)
                && (filter.CompanyFk == null || filter.CompanyFk == 0 || a.CompanyFk == filter.CompanyFk)
                && (filter.SectorFk == null || filter.SectorFk == 0 || a.SectorFk == filter.SectorFk)

                && (filter.ProviderFk == null || filter.ProviderFk == 0 || a.ProviderFk == filter.ProviderFk)
                && (string.IsNullOrEmpty(filter.VoucherCode) || a.VoucherCode == filter.VoucherCode)
                && (string.IsNullOrEmpty(filter.VoucherNo) || a.VoucherNo == filter.VoucherNo)
                && (filter.ApprovedByProvider == null || a.ApprovedByProvider == filter.ApprovedByProvider)
                && (filter.ApprovedBySector == null || a.ApprovedBySector == filter.ApprovedBySector)
                && (filter.NeedActionFromProvider == null || a.NeedActionFromProvider == filter.NeedActionFromProvider)
                && (filter.NeedActionFromSector == null || a.NeedActionFromSector == filter.NeedActionFromSector)
                && (filter.IsVoucherValid == null || a.IsVoucherValid == filter.IsVoucherValid)
                )
                .Include(a => a.SectorFkNavigation)
                .Include(a => a.ProviderFkNavigation)
                .Include(a => a.CompanyFkNavigation).ThenInclude(a=>a.TblVouchers).ThenInclude(a=>a.TblTransactionVouchers);
        }

        public TblVoucher? GetVoucherById(long VoucherId,bool isFull = false)
        {
            if (isFull)
            {
                return _context.TblVouchers.Where(a => a.IsDeleted != true && a.VoucherId == VoucherId)
                 .Include(a => a.SectorFkNavigation)
                .Include(a => a.ProviderFkNavigation)
                .Include(a => a.CompanyFkNavigation).ThenInclude(a => a.TblVouchers).ThenInclude(a => a.TblTransactionVouchers)
                .FirstOrDefault();
            }
            else
            {
                return _context.TblVouchers.Where(a => a.IsDeleted != true && a.VoucherId == VoucherId).FirstOrDefault();
            }
            
        }
        public string UpdateVoucher(TblVoucher entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                _context.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }
        public TblProviderEmployee? GetEmployeeByUserID(string LoginUserid)
        {
            return _context.TblProviderEmployees.Where(a => a.IsDeleted != true && a.LoginUserid == LoginUserid).FirstOrDefault();
        }
        public TblProviderEmployee? GetEmployeeByUserIDFull(string LoginUserid)
        {
            return _context.TblProviderEmployees.Where(a => a.IsDeleted != true && a.LoginUserid == LoginUserid)
                .Include(a=>a.ProviderFkNavigation)
                .FirstOrDefault();
        }

        public TblTransaction? GetTransactionByCode(string code, bool IsFull = false)
        {
            try
            {
                if (IsFull)
                {
                    var entity = _context.TblTransactions
                        .Include(a => a.EmployeeFkNavigation).ThenInclude(a => a.CompanyFkNavigation).ThenInclude(a=>a.SectorFkNavigation)
                        .Include(a => a.ProviderFkNavigation)
                        .Include(a => a.TblTransactionDetails).ThenInclude(a => a.ServiceFkNavigation)
                        .Include(a => a.TblTransactionPayments)
                        .Include(a => a.TblTransactionVouchers).ThenInclude(a => a.VoucherFkNavigation)
                        .FirstOrDefault(a => a.IsDeleted == false && a.TransactionVerficationCode == code);
                    return entity;
                }
                else
                {
                    var entity = _context.TblTransactions.FirstOrDefault(a => a.IsDeleted == false && a.TransactionVerficationCode == code);
                    return entity;
                }
            }
            catch
            {
                return null;
                throw;
            }
        }



    }
}
