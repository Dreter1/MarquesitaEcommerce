using Marquesita.Models.Business;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IDocumentsService
    {
        Task<byte[]> GenerateExcelReport();
        byte[] GeneratePdfSaleReport();
        Task<byte[]> GeneratePdfSaleShop(Sale sale);
        Task<byte[]> GeneratePdfSaleEcommerce(Sale sale);
    }
}
