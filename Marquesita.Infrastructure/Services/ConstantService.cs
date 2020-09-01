using Marquesita.Infrastructure.Interfaces;

namespace Marquesita.Infrastructure.Services
{
    public class ConstantService : IConstantService
    {
        const string rutaServer = @"D:\Documentos\0Dre\0TrabajosFerUPN\10cimo Ciclo\Taller de tesis2\";
        const string rutaProyecto = rutaServer + @"MarquesitaEcommerce\Marquesita.Infrastructure\images\";
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
    }
}
