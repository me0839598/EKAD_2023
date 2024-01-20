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
    public class TblServiceService : ITblServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TblServiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task Add(TblService entity)
        {
            return _unitOfWork.TblServiceRepository.Add(entity);
        }

        // Delete service
        public string DeleteService(int id)
        {
            return _unitOfWork.TblServiceRepository.DeleteService(id);
        }
        public void Delete(int id)
        {
            TblService entity = new TblService() { ServiceId = id };
            _unitOfWork.TblServiceRepository.Delete(entity);
        }

        public string Delete(long id)
        {
            throw new NotImplementedException();
        }


        public Task<TblService> Get(int id)
        {
            return _unitOfWork.TblServiceRepository.Get(id);
        }

        public Task<TblService> Get(long id)
        {
            return _unitOfWork.TblServiceRepository.Get(id);
        }

        public Task<IEnumerable<TblService>> GetAll()
        {
            return _unitOfWork.TblServiceRepository.GetAll();
        }

        public Task<IEnumerable<TblService>> GetAll(GeneralFilter filter)
        {
            return _unitOfWork.TblServiceRepository.GetAll(filter);
        }



        //hhg
        public Task<IEnumerable<TblService>> GetService(Servicefilter filter)
        {
            return _unitOfWork.TblServiceRepository.GetService(filter);
        }



        string IGenericService<TblService>.Delete(int id)
        {
            TblService entity = new TblService() { ServiceId = id };
            return _unitOfWork.TblServiceRepository.Delete(entity);
        }

        string IGenericService<TblService>.Update(TblService entity)
        {
            return _unitOfWork.TblServiceRepository.Update(entity);
        }
    }
}
