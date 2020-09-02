using Marquesita.Models.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategoryList();
    }
}
