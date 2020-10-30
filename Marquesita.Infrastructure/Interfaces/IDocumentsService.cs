using Marquesita.Models.Business;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IDocumentsService
    {
        Task<byte[]> GenerateExcelReport();
        Task<byte[]> GeneratePdfSaleReportAsync();
        Task<byte[]> GenerateSalePDF(Sale sale);
    }
}
