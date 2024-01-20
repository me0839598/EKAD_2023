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
    public class TblAdvertisementTypeYearDayService : ITblAdvertisementTypeYearDayService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TblAdvertisementTypeYearDayService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task Add(TblAdvertisementTypeYearDay entity)
        {
            return _unitOfWork.TblAdvertisementTypeYearDayRepository.Add(entity);
        }

        public string Delete(TblAdvertisementTypeYearDay entity)
        {
            return _unitOfWork.TblAdvertisementTypeYearDayRepository.Delete(entity);
        }

        public string Delete(int id)
        {
            throw new NotImplementedException();
        }

        public string Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<TblAdvertisementTypeYearDay> Get(int id)
        {
            return _unitOfWork.TblAdvertisementTypeYearDayRepository.Get(id);
        }

        public Task<TblAdvertisementTypeYearDay> Get(long id)
        {
            return _unitOfWork.TblAdvertisementTypeYearDayRepository.Get(id);
        }

        public Task<IEnumerable<TblAdvertisementTypeYearDay>> GetAll()
        {
            return _unitOfWork.TblAdvertisementTypeYearDayRepository.GetAll();
        }

        public Task<IEnumerable<TblAdvertisementTypeYearDay>> GetAll(GeneralFilter filter)
        {
            return _unitOfWork.TblAdvertisementTypeYearDayRepository.GetAll(filter);
        }

        public string Update(TblAdvertisementTypeYearDay entity)
        {
            return _unitOfWork.TblAdvertisementTypeYearDayRepository.Update(entity);
        }
    }
}
