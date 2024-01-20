using Ekad.Voucher.domain.context;
using Ekad.Voucher.logic.Repository.Interfaces;
using Ekad.Voucher.logic.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekad.Voucher.logic.Repository.UnitofWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EkadVoucherDbContext _context;
        private ISettingRepository _SettingRepository;
        private ICompanyRepository _CompanyRepository;
        private IProviderRepository _ProviderRepository;
        private ILoginHistoryRepository _LoginHistoryRepository;
        private IUserNotificationRepository _UserNotificationRepository;
        private IAdvertisementRepository _AdvertisementRepository;
        private IInqueryRepository _InqueryRepository;
        private ITblServiceRepository _TblServiceRepository;
        private ITblYearDayRepository _TblYearDayRepository;
        private ITblAdvertisementTypeYearDayRepository _TblAdvertisementTypeYearDayRepository;
        public UnitOfWork(EkadVoucherDbContext elitecrmContext)
        {
            this._context = elitecrmContext;
        }

        public ISettingRepository SettingRepository
        {
            get
            {
                return _SettingRepository ?? (_SettingRepository = new SettingRepository(_context));
            }
        }
        public ICompanyRepository CompanyRepository
        {
            get
            {
                return _CompanyRepository ?? (_CompanyRepository = new CompanyRepository(_context));
            }
        }
        public IProviderRepository ProviderRepository
        {
            get
            {
                return _ProviderRepository ?? (_ProviderRepository = new ProviderRepository(_context));
            }
        }
        // ITblYearDayRepository
        public ITblYearDayRepository TblYearDayRepository
        {
            get
            {
                return _TblYearDayRepository ?? (_TblYearDayRepository = new TblYearDayRepository(_context));
            }
        }
        // ITblAdvertisementTypeYearDayRepository
        public ITblAdvertisementTypeYearDayRepository TblAdvertisementTypeYearDayRepository
        {
            get
            {
                return _TblAdvertisementTypeYearDayRepository ?? (_TblAdvertisementTypeYearDayRepository = new TblAdvertisementTypeYearDayRepository(_context));
            }
        }



        public ILoginHistoryRepository LoginHistoryRepository
        {
            get
            {
                return _LoginHistoryRepository ?? (_LoginHistoryRepository = new LoginHistoryRepository(_context));
            }
        }
        public IUserNotificationRepository UserNotificationRepository
        {
            get
            {
                return _UserNotificationRepository ?? (_UserNotificationRepository = new UserNotificationRepository(_context));
            }
        }
        public IAdvertisementRepository AdvertisementRepository
        {
            get
            {
                return _AdvertisementRepository ?? (_AdvertisementRepository = new AdvertisementRepository(_context));
            }
        }
        public IInqueryRepository InqueryRepository
        {
            get
            {
                return _InqueryRepository ?? (_InqueryRepository = new InqueryRepository(_context));
            }
        }

        public ITblServiceRepository TblServiceRepository
        {
            get
            {
                return _TblServiceRepository ?? (_TblServiceRepository = new TblServiceRepository(_context));
            }
        }


     //   public ITblYearDayRepository TblYearDayRepository => throw new NotImplementedException();

        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
