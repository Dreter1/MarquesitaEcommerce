using Marquesita.Infrastructure.Interfaces;

namespace Marquesita.Infrastructure.Services
{
    public class ConstantService : IConstantService
    {
        const string rutaImgColaborador = "~/Images/Users/Employees/";
        const string rutaImgCliente = "~/Images/Users/Clients/";
        const string rutaImgProducto = "~/Images/Products/";

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
    }
}
