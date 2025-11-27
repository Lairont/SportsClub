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
    public class InventoryService : IInventoryService
    {
        private readonly IBaseStorage<InventoryDb> _inventoryStorage;

        private IMapper _mapper { get; set; }

        private InventoryValidator _validationRules { get; set; }

        private readonly MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AppMappingProfile>();
        });

        public InventoryService(IBaseStorage<InventoryDb> inventoryStorage)
        {
            _inventoryStorage = inventoryStorage;
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
                var inventory = await _inventoryStorage.GetAll().FirstOrDefaultAsync(x => x.Id == id);
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

        public async Task<BaseResponse<List<Inventory>>> GetAllInventories()
        {
            try
            {

                var inventoriesDb = await _inventoryStorage.GetAll()
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
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}