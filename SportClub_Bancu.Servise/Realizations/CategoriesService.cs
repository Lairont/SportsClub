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

        private readonly MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AppMappingProfile>();
        });

        public CategoriesService(IBaseStorage<CategoriesDb> categoryStorage)
        {
            _categoryStorage = categoryStorage;
            _mapper = mapperConfiguration.CreateMapper();
            _validationRules = new CategoryValidator();
        }

        public async Task<BaseResponse<CategoriesDb>> CreateCategory(Categories model)
        {
            try
            {
                await _validationRules.ValidateAndThrowAsync(model);
                var categoryDb = _mapper.Map<CategoriesDb>(model);
                await _categoryStorage.Add(categoryDb);

                return new BaseResponse<CategoriesDb>
                {
                    Data = categoryDb,
                    StatusCode = StatusCode.OK
                };
            }
            catch (ValidationException ex)
            {
                var errorMessages = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));
                return new BaseResponse<CategoriesDb>
                {
                    Description = errorMessages,
                    StatusCode = StatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CategoriesDb>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError
                };
            }
        }

        public async Task<BaseResponse<CategoriesDb>> UpdateCategory(Guid id, Categories model)
        {
            try
            {
                var existingCategory = await _categoryStorage.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (existingCategory == null)
                {
                    return new BaseResponse<CategoriesDb>
                    {
                        Description = "Категория не найдена",
                        StatusCode = StatusCode.NotFound
                    };
                }

                await _validationRules.ValidateAndThrowAsync(model);

                existingCategory.Name = model.Name;
                existingCategory.Description = model.Description;

                await _categoryStorage.Update(existingCategory);

                return new BaseResponse<CategoriesDb>
                {
                    Data = existingCategory,
                    StatusCode = StatusCode.OK
                };
            }
            catch (ValidationException ex)
            {
                var errorMessages = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));
                return new BaseResponse<CategoriesDb>
                {
                    Description = errorMessages,
                    StatusCode = StatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CategoriesDb>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteCategory(Guid id)
        {
            try
            {
                var existingCategory = await _categoryStorage.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (existingCategory == null)
                {
                    return new BaseResponse<bool>
                    {
                        Data = false,
                        Description = "Категория не найдена",
                        StatusCode = StatusCode.NotFound
                    };
                }

                await _categoryStorage.Delete(existingCategory);

                return new BaseResponse<bool>
                {
                    Data = true,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Data = false,
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError
                };
            }
        }

        public async Task<BaseResponse<CategoriesDb>> GetCategoryById(Guid id)
        {
            try
            {
                var category = await _categoryStorage.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                {
                    return new BaseResponse<CategoriesDb>
                    {
                        Description = "Категория не найдена",
                        StatusCode = StatusCode.NotFound
                    };
                }

                return new BaseResponse<CategoriesDb>
                {
                    Data = category,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CategoriesDb>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<CategoriesDb>>> GetAllCategories()
        {
            try
            {
                var categories = await _categoryStorage.GetAll().ToListAsync();

                return new BaseResponse<IEnumerable<CategoriesDb>>
                {
                    Data = categories,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<CategoriesDb>>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError
                };
            }
        }
    }
}