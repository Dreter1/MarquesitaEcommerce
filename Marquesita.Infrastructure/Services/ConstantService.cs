using Marquesita.Infrastructure.Interfaces;

namespace Marquesita.Infrastructure.Services
{
    public class ConstantService : IConstantService
    {
        const string rutaImgColaborador = "~/Images/Users/Employees/";
        const string rutaImgCliente = "~/Images/Users/Clients/";
        const string rutaImgProducto = "~/Images/Products/";

        const string StoreSale = "Tienda";
        const string EcommerceSale = "E-commerce";

        const string SaleStatusInProcess = "En proceso";
        const string SaleStatusConfirmed = "Confirmada";
        const string SaleStatusCanceled = "Cancelada";

        public string RoutePathRootEmployeeImages()
        {
            return rutaImgColaborador;
        }

        public string RoutePathRootClientsImages()
        {
            return rutaImgCliente;
        }

        public string RoutePathRootProductsImages()
        {
            return rutaImgProducto;
        }

        public string Store_Sale()
        {
            return StoreSale;
        }

        public string Ecommerce_Sale()
        {
            return EcommerceSale;
        }

        public string SaleStatus_Process()
        {
            return SaleStatusInProcess;
        }

        public string SaleStatus_Confirmed()
        {
            return SaleStatusConfirmed;
        }

        public string SaleStatus_Canceled()
        {
            return SaleStatusCanceled;
        }
    }
}
