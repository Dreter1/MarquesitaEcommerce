using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Models.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marquesita.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _repository;

        public CategoryService(IRepository<Category> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Category> GetCategoryList()
        {
            return _repository.All();
        }



    }
}
