using Marquesita.Models.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface ISaleService
    {
        IEnumerable<Sale> GetSaleList();
        Task<List<Sale>> GetOrdersAsync(string userName);
    }
}
