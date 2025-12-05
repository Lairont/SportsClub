using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SporClub_Bancu.DAL;
using SportClub_Bancu.Domain.Filter;
using SportClub_Bancu.Domain.ModelsDb;
using SportClub_Bancu.Domain.Response;
using SportClub_Bancu.Domain.Validators;
using SportClub_Bancu.Servise.Interfaces;
using SportsClub_Bancu.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SportClub_Bancu.Servise.Realizations
{
    public class InventoryService : IInventoryService
    {
        private readonly IBaseStorage<InventoryDb> _inventoryStorage;
        private readonly IBaseStorage<PicturesInventoryDb> _picturesStorage;
        private readonly IMapper _mapper; 

        private InventoryValidator _validationRules { get; set; }

        private readonly MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AppMappingProfile>();
        });

        public InventoryService(
            IBaseStorage<InventoryDb> inventoryStorage,
            IBaseStorage<PicturesInventoryDb> picturesStorage)
        {
            _inventoryStorage = inventoryStorage;
            _picturesStorage = picturesStorage;
            _mapper = mapperConfiguration.CreateMapper();
            _validationRules = new InventoryValidator();
        }

        public async Task<BaseResponse<InventoryDb>> CreateInventory(Inventory model)
        {
            try
            {
                await _validationRules.ValidateAndThrowAsync(model);
                var inventory = _mapper.Map<InventoryDb>(model);
                await _inventoryStorage.Add(inventory);

                return new BaseResponse<InventoryDb>
                {
                    Data = inventory,
                    StatusCode = StatusCode.OK
                };
            }
            catch (ValidationException ex)
            {
                var errorMessages = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));
                return new BaseResponse<InventoryDb>
                {
                    Description = errorMessages,
                    StatusCode = StatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<InventoryDb>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError
                };
            }
        }

        public async Task<BaseResponse<InventoryDb>> UpdateInventory(Guid id, Inventory model)
        {
            try
            {
                var existingInventory = await _inventoryStorage.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (existingInventory == null)
                {
                    return new BaseResponse<InventoryDb>
                    {
                        Description = "Инвентарь не найден",
                        StatusCode = StatusCode.NotFound
                    };
                }

                await _validationRules.ValidateAndThrowAsync(model);

                existingInventory.Name = model.Name;
                existingInventory.Notes = model.Notes;
                existingInventory.Count = model.Count;
                existingInventory.Condition = model.Condition;
                existingInventory.InventoryNumber = model.InventoryNumber;
                existingInventory.Price = model.Price;
                existingInventory.PurchaseDate = model.PurchaseDate;
                existingInventory.WarrantyUntil = model.WarrantyUntil;
                existingInventory.CategoryId = model.CategoryId;

                await _inventoryStorage.Update(existingInventory);

                return new BaseResponse<InventoryDb>
                {
                    Data = existingInventory,
                    StatusCode = StatusCode.OK
                };
            }
            catch (ValidationException ex)
            {
                var errorMessages = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));
                return new BaseResponse<InventoryDb>
                {
                    Description = errorMessages,
                    StatusCode = StatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<InventoryDb>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteInventory(Guid id)
        {
            try
            {
                var existingInventory = await _inventoryStorage.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (existingInventory == null)
                {
                    return new BaseResponse<bool>
                    {
                        Data = false,
                        Description = "Инвентарь не найден",
                        StatusCode = StatusCode.NotFound
                    };
                }

                await _inventoryStorage.Delete(existingInventory);

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

        public async Task<BaseResponse<InventoryDb>> GetInventoryById(Guid id)
        {
            try
            {
                

                var inventory = await _inventoryStorage.GetAll()
                    .Include(x => x.PicturesInventoryDb) 
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (inventory == null)
                {
                    return new BaseResponse<InventoryDb>
                    {
                        Description = "Инвентарь не найден",
                        StatusCode = StatusCode.NotFound
                    };
                }

                return new BaseResponse<InventoryDb>
                {
                    Data = inventory,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<InventoryDb>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError
                };
            }
        }




        public async Task<BaseResponse<PicturesInventory>> GetPictur(Guid Id)
        {
            try
            {
                var picturesinventory = await _picturesStorage.GetAll().FirstOrDefaultAsync(p => p.InventoryId == Id);


                var result = _mapper.Map<PicturesInventory>(picturesinventory);

                if (result == null)
                {
                    return new BaseResponse<PicturesInventory>
                    {
                        Description = "Найдено 0 элементов",
                        StatusCode = StatusCode.NotFound
                    };
                }

                return new BaseResponse<PicturesInventory>
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<PicturesInventory>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError
                };
            }
        }

        public async Task<BaseResponse<List<Inventory>>> GetAllInventories(Guid IdInventory)
        {


            try
            {
                var inventoriesDb = await _inventoryStorage.GetAll()
                    .Include(x => x.PicturesInventoryDb)
                    .OrderBy(p => p.CreatedAt)
                    .ToListAsync();

                var result = _mapper.Map<List<Inventory>>(inventoriesDb);

                if (result.Count == 0)
                {
                    return new BaseResponse<List<Inventory>>
                    {
                        Description = "Найдено 0 элементов",
                        StatusCode = StatusCode.NotFound
                    };
                }

                return new BaseResponse<List<Inventory>>
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Inventory>>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError
                };
            }
        }


        public BaseResponse<List<Inventory>> GetInventoryByFilter(InventoryFilter filter)
        {
            try
            {
                // Получаем все данные (как у тебя и было)
                var inventoryList = GetAllInventories(filter.IdInventory).Result.Data;

                if (filter != null && inventoryList != null)
                {
                    // 1. Фильтрация по Цене (твоя старая логика)
                    if (filter.PriceMax != 500000 || filter.PriceMin != 0)
                    {
                        inventoryList = inventoryList
                            .Where(f => f.Price <= filter.PriceMax && f.Price >= filter.PriceMin)
                            .ToList();
                    }

                    // 2. Фильтрация по Категориям (НОВАЯ ЛОГИКА)
                    // Проверяем, выбрал ли пользователь хоть одну категорию
                    if (filter.CategoryIds != null && filter.CategoryIds.Any())
                    {
                        // Оставляем только те товары, чей CategoryId есть в списке выбранных
                        inventoryList = inventoryList
                            .Where(item => filter.CategoryIds.Contains(item.CategoryId))
                            .ToList();
                    }
                }

                return new BaseResponse<List<Inventory>>
                {
                    Data = inventoryList,
                    Description = "Отфильтрованные данные",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Inventory>>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}