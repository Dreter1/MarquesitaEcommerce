namespace Marquesita.Infrastructure.Interfaces
{
    public interface IDocumentsService
    {
        byte[] GenerateExcelReport();
        byte[] GeneratePdfSale();
    }
}
