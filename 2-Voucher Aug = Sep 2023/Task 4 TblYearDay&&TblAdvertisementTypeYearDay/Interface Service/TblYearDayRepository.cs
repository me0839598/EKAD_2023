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
    public class TblYearDayRepository : ITblYearDayRepository
    {
        protected readonly EkadVoucherDbContext _context;
        public TblYearDayRepository(EkadVoucherDbContext Context)
        {
            _context = Context;
        }
        public async Task Add(TblYearDay entity)
        {
            await _context.TblYearDays.AddAsync(entity);
            _context.SaveChanges();
        }

         string IGenericRepository<TblYearDay>.Delete(TblYearDay entity)
        {
            try
            {
                _context.TblYearDays.Remove(entity);
                _context.SaveChanges();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw;
            }

        }

        public async Task<TblYearDay> Get(int id)
        {
            var list = await _context.TblYearDays.Where(x => x.YearDayId == id).FirstOrDefaultAsync();
            return list;
        }

        public Task<TblYearDay> Get(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TblYearDay>> GetAll() 
        {
            var list = await _context.TblYearDays.ToListAsync();
            return list;
        }

        public Task<IEnumerable<TblYearDay>> GetAll(GeneralFilter filter)
        {
            throw new NotImplementedException();
        }

        string IGenericRepository<TblYearDay>.Update(TblYearDay entity)
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
