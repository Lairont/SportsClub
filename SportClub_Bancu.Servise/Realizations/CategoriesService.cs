using AutoMapper;
using SportClub_Bancu.Domain.ModelsDb;
using SportClub_Bancu.Servise.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using SporClub_Bancu.DAL;
using SportClub_Bancu.Domain.Response;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using SportClub_Bancu.Domain.Validators;

namespace SportClub_Bancu.Servise.Realizations
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IBaseStorage<CategoriesDb> _categoryStorage;

        private IMapper _mapper { get; set; }

        private CategoryValidator _validationRules { get; set; }

        MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AppMappingProfile>();
        });

        public CategoriesService(IBaseStorage<CategoriesDb> categoryStorage)
        {
            _categoryStorage = categoryStorage;
            _mapper = mapperConfiguration.CreateMapper();
            _validationRules = new CategoryValidator();
        }

        public BaseResponse<List<Categories>> GetAllCategories()
        {
            try
            {
                var categoriesDb = _categoryStorage.GetAll().OrderBy(x => x.CreatedAt).ToList();
                var result = _mapper.Map<List<Categories>>(categoriesDb);
                if (result.Count == 0)
                {
                    return new BaseResponse<List<Categories>>()
                    {
                        Description = "Найдено 0 элементов",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<List<Categories>>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Categories>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}