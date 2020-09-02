using System;
using System.Collections.Generic;
using System.Text;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IConstantService
    {
        string RoutePathEmployeeImages();
        string RoutePathClientsImages();
        string RoutePathProductsImages();
        string RoutePathRootEmployeeImages();
        string RoutePathRootClientsImages();
        string RoutePathRootProductsImages();
    }
}
