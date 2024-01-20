using Ekad.Voucher.domain.context;
using Ekad.Voucher.domain.Models;
using Ekad.Voucher.logic.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Ekad.Voucher.logic.Repository.Repositories
{
    public class TblServiceRepository : ITblServiceRepository
    {
        protected readonly EkadVoucherDbContext _context;
        public TblServiceRepository(EkadVoucherDbContext Context)
        {
            _context = Context;
        }
        public async Task Add(TblService entity)
        {
            await _context.TblServices.AddAsync(entity);
            _context.SaveChanges();
        }
        // return _context.TblSectors.Where(a => a.IsDeleted != true && a.SectorId == id).Include(a=>a.TblCompanies).Include(a => a.TblVouchers).Include(a => a.TblAdvertisements).FirstOrDefault();

        public async Task<TblService> Get(int id)
        {
            var list = await _context.TblServices.Where(x => x.ServiceId == id).Include(a => a.TblProviderServices).Include(a => a.TblTransactionDetails)
                .FirstOrDefaultAsync();
            return list;
        }

        public async Task<IEnumerable<TblService>> GetService(Servicefilter filter)
        {
            return await _context.TblServices.Where(a =>
                (filter.ServiceType == null || a.ServiceTypeFk == filter.ServiceType)
               && (string.IsNullOrEmpty(filter.Servicename) || a.ServiceNameAr.Contains(filter.Servicename) || a.ServiceNameEn.Contains(filter.Servicename)
               )).Include(a => a.ServiceTypeFkNavigation).ToListAsync();
        }

        public async Task<TblService> Get(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TblService>> GetAll()
        {
            var list = await _context.TblServices.Include(a=>a.ServiceTypeFkNavigation).ToListAsync();
            return list;
        }

        public async Task<IEnumerable<TblService>> GetAll(GeneralFilter filter)
        {
            var list = await _context.TblServices.Where(a =>
            (!filter.parentid.HasValue || a.ServiceTypeFk == filter.parentid) &&
            (string.IsNullOrEmpty(filter.textLike) || a.ServiceNameAr.Contains(filter.textLike) || a.ServiceNameEn.Contains(filter.textLike))

            ).ToListAsync();
            return list;
        }
        // DeleteServic
        public string DeleteService(int id)
        {
            var service = Get(id).Result;
            if (service.TblTransactionDetails.Count()>0 || service.TblProviderServices.Count() > 0)
            {
              return "failed";
            }
            else
            {
                _context.TblServices.Remove(service);
                _context.SaveChanges();
                return "success";
            }

        }


        string IGenericRepository<TblService>.Delete(TblService entity)
        {
            try
            {
                _context.TblServices.Remove(entity);
                _context.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }

        }

        string IGenericRepository<TblService>.Update(TblService entity)
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
    }
}
