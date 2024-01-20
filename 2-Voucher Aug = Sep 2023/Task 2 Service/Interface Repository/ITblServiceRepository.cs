using Ekad.Voucher.domain.context;
using Ekad.Voucher.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekad.Voucher.logic.Repository.Interfaces
{
    public interface ITblServiceRepository : IGenericRepository<TblService>
    {
        Task<IEnumerable<TblService>> GetService(Servicefilter filter);
        //Delete Service
        string DeleteService(int id);

    }
}
