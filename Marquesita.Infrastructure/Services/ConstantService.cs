using Marquesita.Infrastructure.Interfaces;

namespace Marquesita.Infrastructure.Services
{
    public class ConstantService : IConstantService
    {
        const string rutaServer = @"D:\Documentos\0Dre\0TrabajosFerUPN\10cimo Ciclo\Taller de tesis2\";
        const string rutaProyecto = rutaServer + @"MarquesitaEcommerce\Marquesita.Infrastructure\images\";
        const string rutaImgColaborador = "~/Images/Users/";
        const string rutaImgCliente = "~/Images/Users/";
        const string rutaImgProducto = "~/Images/Products/";

        public string RoutePathEmployeeImages()
        {    
            return rutaProyecto + @"\Users\Employees\";
        }

        public string RoutePathClientsImages()
        {
            return rutaProyecto + @"\Users\Clients\";
        }

        public string RoutePathProductsImages()
        {
            return rutaProyecto + @"\Products\";
        }

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
