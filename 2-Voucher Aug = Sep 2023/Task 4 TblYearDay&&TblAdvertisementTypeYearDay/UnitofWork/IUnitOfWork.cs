using Ekad.Voucher.logic.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekad.Voucher.logic.Repository.UnitofWork
{
    public interface IUnitOfWork : IDisposable
    {
        ISettingRepository SettingRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        IProviderRepository ProviderRepository { get; }
        ILoginHistoryRepository LoginHistoryRepository { get; }
        IUserNotificationRepository UserNotificationRepository { get; }
        IAdvertisementRepository AdvertisementRepository { get; }
        IInqueryRepository InqueryRepository { get; }
        ITblServiceRepository TblServiceRepository { get; }
        ITblYearDayRepository TblYearDayRepository { get; }
        ITblAdvertisementTypeYearDayRepository TblAdvertisementTypeYearDayRepository { get; }


        int Complete();
    }
}
