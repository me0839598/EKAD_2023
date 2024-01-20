using Ekad.Voucher.domain.context;
using Ekad.Voucher.domain.Models;
using Ekad.Voucher.logic.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekad.Voucher.logic.Repository.Repositories
{
    public class TblAdvertisementTypeYearDayRepository : ITblAdvertisementTypeYearDayRepository
    {
        protected readonly EkadVoucherDbContext _context;

        public TblAdvertisementTypeYearDayRepository(EkadVoucherDbContext Context)
        {
            _context = Context;
        }

        public async Task Add(TblAdvertisementTypeYearDay entity)
        {
            await _context.TblAdvertisementTypeYearDays.AddAsync(entity);
            _context.SaveChanges();
        }

        string IGenericRepository<TblAdvertisementTypeYearDay>.Delete(TblAdvertisementTypeYearDay entity)
        {
            try
            {
                _context.TblAdvertisementTypeYearDays.Remove(entity);
                _context.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }

        }

        public async Task<TblAdvertisementTypeYearDay>Get(int id)
        {
            var list = await _context.TblAdvertisementTypeYearDays.Where(x => x.AdvertisementYearDayId == id).FirstOrDefaultAsync();
            return list;
        }

        public Task<TblAdvertisementTypeYearDay> Get(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TblAdvertisementTypeYearDay>> GetAll()
        {
            var list = await _context.TblAdvertisementTypeYearDays.ToListAsync();
            return list;
        }
    

        public Task<IEnumerable<TblAdvertisementTypeYearDay>> GetAll(GeneralFilter filter)
        {
            throw new NotImplementedException();
        }

        string IGenericRepository<TblAdvertisementTypeYearDay>.Update(TblAdvertisementTypeYearDay entity)
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
