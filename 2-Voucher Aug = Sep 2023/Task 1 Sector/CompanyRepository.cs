using Ekad.Voucher.domain.context;
using Ekad.Voucher.domain.Models;
using Ekad.Voucher.domain.Models.VM;
using Ekad.Voucher.domain.Resources;
using Ekad.Voucher.logic.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekad.Voucher.logic.Repository.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        protected readonly EkadVoucherDbContext _context;
        public CompanyRepository(EkadVoucherDbContext Context)
        {
            _context = Context;
        }

        public async System.Threading.Tasks.Task Add(TblCompany entity)
        {
            await _context.TblCompanies.AddAsync(entity);
            _context.SaveChanges();
        }

        //ADD Sector 
        
        public async System.Threading.Tasks.Task AddSect(TblSector entity)
        {
            await _context.TblSectors.AddAsync(entity);
            _context.SaveChanges();
        }
        //public async System.Threading.Tasks.Task AddSector(TblSector entity)
        //{
        //    await _context.TblSectors.AddAsync(entity);
        //    _context.SaveChanges();
        //}


        // Delete Sector
        public string Deletesector(int id)
        {
            var sec = GetSectorID(id);
            if (sec.TblCompanies.Count() > 0 || sec.TblVouchers.Count() > 0 || sec.TblAdvertisements.Count() > 0)
            {
                return "failed";
            }
            else
            {
                sec.IsDeleted = true;
                string res = Updatesec(sec);
                return res;
            }

        }




        public string Delete(TblCompany entity)
        {
            try
            {
                _context.TblCompanies.Remove(entity);
                _context.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }

        public async Task<TblCompany> Get(int id)
        {
            return await _context.TblCompanies.Where(a => a.CompanyId == id)
                .FirstOrDefaultAsync();
        }

        public TblSector? GetSectorID(int id)
        {
            return _context.TblSectors.Where(a => a.IsDeleted != true && a.SectorId == id).Include(a=>a.TblCompanies).Include(a => a.TblVouchers).Include(a => a.TblAdvertisements).FirstOrDefault();
        }

        //public async Task<TblSector> GetSectorID(int id)
        //{
        //    return await _context.TblSectors.Where(a => a.SectorId == id)
        //        .FirstOrDefaultAsync();
        //}



        public Task<TblCompany> Get(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TblCompany>> GetAll()
        {
            return await _context.TblCompanies.Where(a => a.IsDeleted != true).Include(s => s.SectorFkNavigation).Include(c=>c.CityFkNavigation).ToListAsync();
        }

        public async Task<IEnumerable<TblCompany>> GetAll(GeneralFilter filter)
        {
            return await _context.TblCompanies.Where(a => a.IsDeleted != true
            && (filter.id == null || a.CompanyId == filter.id)
            && (filter.parentid == null || a.SectorFk == filter.parentid)
            && (string.IsNullOrEmpty(filter.textLike) || a.CompanyNameAr.Contains(filter.textLike) || a.CompanyNameEn.Contains(filter.textLike))
            ).ToListAsync();
        }
        public IEnumerable<TblCompany> GetCompanies(CompanyFilter filter)
        {
            return  _context.TblCompanies.Where(a => a.IsDeleted != true
            && (filter.country == null || a.CountryFk == filter.country)
            && (filter.city == null || a.CityFk == filter.city)
            && (filter.sector == null || a.SectorFk == filter.sector)
            && (string.IsNullOrEmpty(filter.CompanyName) || a.CompanyNameAr.Contains(filter.CompanyName) || a.CompanyNameEn.Contains(filter.CompanyName))
            ).Include(s => s.SectorFkNavigation).Include(c => c.CityFkNavigation).ToList();
        }
        // Getsector
        public IEnumerable<TblSector> Getsector(sectorfilter filter)
        {
            return _context.TblSectors.Where(a => a.IsDeleted != true
                 && (filter.sectorid == null || a.SectorId == filter.sectorid)
                 && (string.IsNullOrEmpty(filter.sectorname) || a.SectorNameAr.Contains(filter.sectorname) || a.SectorNameEn.Contains(filter.sectorname))
                 ).ToList();
        }
       
        public TblEmployee? GetEmployeeByUserID(string LoginUserid)
        {
            return _context.TblEmployees.Where(a => a.IsDeleted != true && a.LoginUserid == LoginUserid).FirstOrDefault();
        }
        public TblEmployee? GetEmployeeByUserIDFull(string LoginUserid)
        {
            return _context.TblEmployees.Where(a => a.IsDeleted != true && a.LoginUserid == LoginUserid).FirstOrDefault();
        }
        public IEnumerable<TblEmployee> GetEmployeeList(EmployeeFilter filter)
        {
            return _context.TblEmployees.Where(a => a.IsDeleted != true
                && (filter.EmployeeId == null || a.EmployeeId == filter.EmployeeId)
                && (filter.IsActive == null || a.IsActive == filter.IsActive)
                && (filter.IsRepresentative == null || a.IsRepresentative == filter.IsRepresentative)
                && (filter.CompanyFk == null || a.CompanyFk == filter.CompanyFk)
                && (filter.NationalityFk == null || a.NationalityFk == filter.NationalityFk)
                && (string.IsNullOrEmpty(filter.LoginUserid) || a.LoginUserid == filter.LoginUserid)
                && (string.IsNullOrEmpty(filter.EmployeeEmail) || a.EmployeeEmail == filter.EmployeeEmail)
                && (string.IsNullOrEmpty(filter.EmployeeMobile) || a.EmployeeMobile == filter.EmployeeMobile)
                && (string.IsNullOrEmpty(filter.ActivationCode) || a.ActivationCode == filter.ActivationCode)
                && (string.IsNullOrEmpty(filter.LoginBy) || a.EmployeeEmail == filter.LoginBy || a.EmployeeMobile == filter.LoginBy)
                && (
                string.IsNullOrEmpty(filter.textLike) || a.EmployeeNameAr.Contains(filter.textLike) || a.EmployeeNameEn.Contains(filter.textLike)
                || a.EmployeeMobile.Contains(filter.textLike) || a.EmployeeJob.Contains(filter.textLike) || a.EmployeeEmail.Contains(filter.textLike)
                )
                );
        }

        public IEnumerable<TblSector> GetsectorList(GeneralFilter filter)
        {
            return _context.TblSectors.Where(a => a.IsDeleted != true
                 && (filter.id == null || a.SectorId == filter.id)
                 && (string.IsNullOrEmpty(filter.textLike) || a.SectorNameAr.Contains(filter.textLike) || a.SectorNameEn.Contains(filter.textLike))
                 ).ToList();
        }

        public string Update(TblCompany entity)
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
        public string updateEmployee(TblEmployee entity)
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

        public string Updatesec(TblSector entity)
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

        public string ValidateNewTransaction(TransactionVM obj, TblEmployee userobj)
        {
            try
            {
                if (obj.TotalTransctionValue==0)
                {
                    return Resource.ThereIsNoTransactionValue;
                }
                if (obj.TotalTransctionValue != (obj.PayWithCashValue + obj.PayWithWalletValue))
                {
                    return Resource.TheInvoiceTotalValueNotEqualPaymentValue;
                }

                var voucherlist = _context.TblVouchers.Where(a => a.ProviderFk == obj.ProviderId && a.IsVoucherValid &&
                a.TblVoucherCompanyRepEmps.Any(x => x.EmpFk == userobj.EmployeeId)).Select(a=>a.VoucherId).ToList();
                if (!voucherlist.Any())
                {
                    return Resource.ThereIsNoValidVouchers;
                }
                var TblVoucherCompanyRepEmp = _context.TblVoucherCompanyRepEmps
                    .Where(a => a.EmpFk == userobj.EmployeeId  && voucherlist.Contains(a.VoucherFk));

                decimal totalincomebalance = TblVoucherCompanyRepEmp==null?0:
                    TblVoucherCompanyRepEmp.Sum(a => a.Amount * a.Factor);
                if (totalincomebalance<=0)
                {
                    return Resource.YourVoucherBalanceLessThanTheWalletPaymentValue;
                }
                decimal totalOutcomebalance = _context.TblTransactionVouchers.Where(x => x.VoucherFk.HasValue && voucherlist.Contains(x.VoucherFk.Value)
                && x.TransactionFkNavigation.EmployeeFk == userobj.EmployeeId && x.TransactionFkNavigation.IsActive)
                    .Sum(a => a.VoucherAnount);

                    //_context.TblTransactions
                    //.Where(a => a.EmployeeFk == userobj.EmployeeId && a.IsActive && a.TblTransactionVouchers.Any(x => x.VoucherFk.HasValue && voucherlist.Contains(x.VoucherFk.Value)) )
                    //.Sum(a => a.TransactionValue * a.TransactionFactor);

                decimal availableBalance = totalincomebalance - totalOutcomebalance;
                if (availableBalance< (obj.PayWithWalletValue))
                {
                    return Resource.YourVoucherBalanceLessThanTheWalletPaymentValue;
                }


                return "valid";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }
        }

        public List<EmpVoucherTotalVM>? GetVoucherForNewTransaction(TransactionVM obj, TblEmployee userobj)
        {
            try
            {
                decimal valueToDeductFromVoucher = obj.PayWithWalletValue;
                var voucherlist = _context.TblVouchers.Where(a => a.ProviderFk == obj.ProviderId && a.IsVoucherValid &&
                a.TblVoucherCompanyRepEmps.Any(x => x.EmpFk == userobj.EmployeeId)).Select(a => a.VoucherId).ToList();
                if (!voucherlist.Any())
                {
                    return null;
                }

                var TblVoucherCompanyRepEmp = _context.TblVoucherCompanyRepEmps
                    .Where(a => a.EmpFk == userobj.EmployeeId && voucherlist.Contains(a.VoucherFk));


                var EmpVoucherTotalList = TblVoucherCompanyRepEmp.GroupBy(a => a.VoucherFk).Select(a => new EmpVoucherTotalVM()
                {
                    VoucherId = a.Key,
                    EmpId = a.FirstOrDefault().EmpFk,
                    TotalValue = a.Sum(x => x.Amount * x.Factor)
                }).ToList();

                if (!EmpVoucherTotalList.Any())
                {
                    return null;
                }
                List<EmpVoucherTotalVM> res=new List<EmpVoucherTotalVM>();

                foreach (var item in EmpVoucherTotalList)
                {
                    if (valueToDeductFromVoucher>0)
                    {
                        if (valueToDeductFromVoucher <= item.TotalValue)
                        {
                            res.Add(new EmpVoucherTotalVM()
                            {
                                EmpId = item.EmpId,
                                VoucherId = item.VoucherId,
                                TotalValue = valueToDeductFromVoucher
                            });
                            return res;
                        }
                        else
                        {
                            res.Add(new EmpVoucherTotalVM()
                            {
                                EmpId = item.EmpId,
                                VoucherId = item.VoucherId,
                                TotalValue = item.TotalValue
                            });
                            valueToDeductFromVoucher = valueToDeductFromVoucher - item.TotalValue;
                        }
                    }
                    
                }
                return res;
            }
            catch
            {
                return null;
                throw;
            }
        }
        public int Get_max_Transaction_no(int EmployeeFk)
        {
            int? Id = _context.TblTransactions.Where(a=>a.EmployeeFk== EmployeeFk).Max(u => Convert.ToInt32( u.TransactionNo));

            return Id.HasValue ? Id.Value + 1 : 1;
        }
        public TblTransaction? AddTransaction(TblTransaction entity)
        {
            try
            {
                var nextnumber= Get_max_Transaction_no(entity.EmployeeFk);
                entity.TransactionNo = nextnumber.ToString();
                _context.TblTransactions.AddAsync(entity);
                _context.SaveChanges();
                return entity;
            }
            catch 
            {
                return null;
                throw;
            }
        }

        public TblTransaction? GetTransactionByID(long Id,bool IsFull=false)
        {
            try
            {
                if (IsFull)
                {
                    var entity = _context.TblTransactions
                        .Include(a=>a.EmployeeFkNavigation).ThenInclude(a=>a.CompanyFkNavigation)
                        .Include(a=>a.ProviderFkNavigation)
                        .Include(a=>a.TblTransactionDetails).ThenInclude(a=>a.ServiceFkNavigation)
                        .Include(a=>a.TblTransactionPayments)
                        .Include(a=>a.TblTransactionReviews)
                        .Include(a=>a.TblTransactionVouchers).ThenInclude(a=>a.VoucherFkNavigation)
                        .FirstOrDefault(a => a.IsDeleted == false && a.TransactionId == Id);
                    return entity;
                }
                else
                {
                    var entity = _context.TblTransactions.FirstOrDefault(a => a.IsDeleted == false && a.TransactionId == Id);
                    return entity;
                }
            }
            catch
            {
                return null;
                throw;
            }
        }

        public IEnumerable<TblTransaction> GetTransactionList(TransactionFilter filter, bool IsFull = false)
        {
            try
            {
                if (IsFull)
                {
                    var entity = _context.TblTransactions
                        .Where(a=>
                        (!filter.TransactionId.HasValue || a.TransactionId== filter.TransactionId)
                        && (!filter.ProviderFk.HasValue || a.ProviderFk == filter.ProviderFk)
                        && (!filter.EmployeeFk.HasValue || a.EmployeeFk == filter.EmployeeFk)
                        && (!filter.IsActive.HasValue || a.IsActive == filter.IsActive)
                        && (string.IsNullOrEmpty(filter.TransactionNo) || a.TransactionNo == filter.TransactionNo)
                        && (string.IsNullOrEmpty(filter.TransactionVerficationCode) || a.TransactionVerficationCode == filter.TransactionVerficationCode)
                        && (string.IsNullOrEmpty(filter.EmployeeUserId) || a.EmployeeFkNavigation.LoginUserid == filter.EmployeeUserId)
                        )
                        .Include(a => a.EmployeeFkNavigation).ThenInclude(a => a.CompanyFkNavigation)
                        .Include(a => a.ProviderFkNavigation)
                        .Include(a => a.TblTransactionDetails).ThenInclude(a => a.ServiceFkNavigation)
                        .Include(a => a.TblTransactionPayments)
                        .Include(a => a.TblTransactionReviews)
                        .Include(a => a.TblTransactionVouchers).ThenInclude(a => a.VoucherFkNavigation);
                    return entity;
                }
                else
                {
                    var entity = _context.TblTransactions
                        .Where(a =>
                        (!filter.TransactionId.HasValue || a.TransactionId == filter.TransactionId)
                        && (!filter.ProviderFk.HasValue || a.ProviderFk == filter.ProviderFk)
                        && (!filter.EmployeeFk.HasValue || a.EmployeeFk == filter.EmployeeFk)
                        && (!filter.IsActive.HasValue || a.IsActive == filter.IsActive)
                        && (string.IsNullOrEmpty(filter.TransactionNo) || a.TransactionNo == filter.TransactionNo)
                        && (string.IsNullOrEmpty(filter.TransactionVerficationCode) || a.TransactionVerficationCode == filter.TransactionVerficationCode)
                        && (string.IsNullOrEmpty(filter.EmployeeUserId) || a.EmployeeFkNavigation.LoginUserid == filter.EmployeeUserId)
                        );
                    return entity;
                }
            }
            catch
            {
                return null;
                throw;
            }
        }

        //Task ICompanyRepository.AddSector(TblSector entity)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
