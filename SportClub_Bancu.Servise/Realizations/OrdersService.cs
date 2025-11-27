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
    public class OrdersService : IOrdersService
    {
        private readonly IBaseStorage<OrdersDb> _orderStorage;

        private IMapper _mapper { get; set; }

        private OrderValidator _validationRules { get; set; }

        private readonly MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AppMappingProfile>();
        });

        public OrdersService(IBaseStorage<OrdersDb> orderStorage)
        {
            _orderStorage = orderStorage;
            _mapper = mapperConfiguration.CreateMapper();
            _validationRules = new OrderValidator();
        }

        public async Task<BaseResponse<OrdersDb>> CreateOrder(Orders model)
        {
            try
            {
                await _validationRules.ValidateAndThrowAsync(model);
                var orderDb = _mapper.Map<OrdersDb>(model);
                await _orderStorage.Add(orderDb);

                return new BaseResponse<OrdersDb>
                {
                    Data = orderDb,
                    StatusCode = StatusCode.OK
                };
            }
            catch (ValidationException ex)
            {
                var errorMessages = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));
                return new BaseResponse<OrdersDb>
                {
                    Description = errorMessages,
                    StatusCode = StatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<OrdersDb>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError
                };
            }
        }

        public async Task<BaseResponse<OrdersDb>> UpdateOrder(Guid id, Orders model)
        {
            try
            {
                var existingOrder = await _orderStorage.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (existingOrder == null)
                {
                    return new BaseResponse<OrdersDb>
                    {
                        Description = "Заказ не найден",
                        StatusCode = StatusCode.NotFound
                    };
                }

                await _validationRules.ValidateAndThrowAsync(model);

                existingOrder.UserId = model.UserId;
                existingOrder.InventoryId = model.InventoryId;
                existingOrder.Quantity = model.Quantity;
                existingOrder.Status = model.Status;
                existingOrder.IssuedAt = model.IssuedAt;
                existingOrder.ReturnedAt = model.ReturnedAt;
                existingOrder.DueDate = model.DueDate;
                existingOrder.Notes = model.Notes;

                await _orderStorage.Update(existingOrder);

                return new BaseResponse<OrdersDb>
                {
                    Data = existingOrder,
                    StatusCode = StatusCode.OK
                };
            }
            catch (ValidationException ex)
            {
                var errorMessages = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));
                return new BaseResponse<OrdersDb>
                {
                    Description = errorMessages,
                    StatusCode = StatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<OrdersDb>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteOrder(Guid id)
        {
            try
            {
                var existingOrder = await _orderStorage.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (existingOrder == null)
                {
                    return new BaseResponse<bool>
                    {
                        Data = false,
                        Description = "Заказ не найден",
                        StatusCode = StatusCode.NotFound
                    };
                }

                await _orderStorage.Delete(existingOrder);

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

        public async Task<BaseResponse<OrdersDb>> GetOrderById(Guid id)
        {
            try
            {
                var order = await _orderStorage.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (order == null)
                {
                    return new BaseResponse<OrdersDb>
                    {
                        Description = "Заказ не найден",
                        StatusCode = StatusCode.NotFound
                    };
                }

                return new BaseResponse<OrdersDb>
                {
                    Data = order,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<OrdersDb>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<OrdersDb>>> GetAllOrders()
        {
            try
            {
                var orders = await _orderStorage.GetAll().ToListAsync();

                return new BaseResponse<IEnumerable<OrdersDb>>
                {
                    Data = orders,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<OrdersDb>>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<OrdersDb>>> GetOrdersByUserId(Guid userId)
        {
            try
            {
                var orders = await _orderStorage.GetAll().Where(x => x.UserId == userId).ToListAsync();

                return new BaseResponse<IEnumerable<OrdersDb>>
                {
                    Data = orders,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<OrdersDb>>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError
                };
            }
        }

        public async Task<BaseResponse<IEnumerable<OrdersDb>>> GetOrdersByInventoryId(Guid inventoryId)
        {
            try
            {
                var orders = await _orderStorage.GetAll().Where(x => x.InventoryId == inventoryId).ToListAsync();

                return new BaseResponse<IEnumerable<OrdersDb>>
                {
                    Data = orders,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<OrdersDb>>
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalError
                };
            }
        }
    }
}