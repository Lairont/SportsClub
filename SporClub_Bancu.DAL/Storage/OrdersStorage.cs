using Microsoft.EntityFrameworkCore;
using SportClub_Bancu.Domain.ModelsDb;
using SporClub_Bancu.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub_Bancu.DAL.Storage
{
    public class OrdersStorage : IBaseStorage<OrdersDb>
    {
        private readonly ApplicationDbContext _db;

        public OrdersStorage(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Add(OrdersDb item)
        {
            await _db.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(OrdersDb item)
        {
            _db.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task<OrdersDb> Get(Guid id)
        {
            return await _db.OrdersDb.FirstOrDefaultAsync(o => o.Id == id);
        }

        public IQueryable<OrdersDb> GetAll()
        {
            return _db.OrdersDb;
        }

        public async Task<OrdersDb> Update(OrdersDb item)
        {
            _db.OrdersDb.Update(item);
            await _db.SaveChangesAsync();
            return item;
        }
    }
}
