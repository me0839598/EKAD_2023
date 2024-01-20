using Ekad.Voucher.domain.context;
using Ekad.Voucher.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekad.Voucher.logic.Service.Interfaces
{
    public interface ITblServiceService : IGenericService<TblService>
    {
        Task<IEnumerable<TblService>> GetService(Servicefilter filter);
        // DeleteService
        string DeleteService(int id);

    }
}
