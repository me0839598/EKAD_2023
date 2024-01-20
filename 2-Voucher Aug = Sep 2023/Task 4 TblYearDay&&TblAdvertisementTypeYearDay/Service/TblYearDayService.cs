using Ekad.Voucher.domain.context;
using Ekad.Voucher.domain.Models;
using Ekad.Voucher.logic.Repository.UnitofWork;
using Ekad.Voucher.logic.Repository.Interfaces;
using Ekad.Voucher.logic.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekad.Voucher.logic.Service.Services
{
    public class TblYearDayService : ITblYearDayService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TblYearDayService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task Add(TblYearDay entity)
        {
            return _unitOfWork.TblYearDayRepository.Add(entity);
        }

        public string Delete(TblYearDay entity)
        {
            return _unitOfWork.TblYearDayRepository.Delete(entity);
        }
        
        public Task<TblYearDay> Get(int id)
        {
            return _unitOfWork.TblYearDayRepository.Get(id);
        }

        public Task<TblYearDay> Get(long id)
        {
            return _unitOfWork.TblYearDayRepository.Get(id);
        }

        public Task<IEnumerable<TblYearDay>> GetAll()
        {
            return _unitOfWork.TblYearDayRepository.GetAll();
        }

        public Task<IEnumerable<TblYearDay>> GetAll(GeneralFilter filter)
        {
            return _unitOfWork.TblYearDayRepository.GetAll(filter);
        }

        public string Update(TblYearDay entity)
        {
            return _unitOfWork.TblYearDayRepository.Update(entity);
        }
    }
}
