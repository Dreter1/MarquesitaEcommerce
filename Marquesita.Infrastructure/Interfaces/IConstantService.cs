namespace Marquesita.Infrastructure.Interfaces
{
    public interface IConstantService
    {
        string RoutePathRootEmployeeImages();
        string RoutePathRootClientsImages();
        string RoutePathRootProductsImages();
        string Store_Sale();
        string Ecommerce_Sale();
        string SaleStatus_Process();
        string SaleStatus_Confirmed();
        string SaleStatus_Canceled();
    }
}
