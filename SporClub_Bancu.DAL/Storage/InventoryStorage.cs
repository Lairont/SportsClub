using Microsoft.EntityFrameworkCore;
using SporClub_Bancu.DAL;
using SportClub_Bancu.Domain.ModelsDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportClub_Bancu.DAL.Storage
{
    public class InventoryStorage : IBaseStorage<InventoryDb>
    {
        private readonly ApplicationDbContext _db;

        public InventoryStorage(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Add(InventoryDb item)
        {
            await _db.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(InventoryDb item)
        {
            _db.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task<InventoryDb> Get(Guid id)
        {
            return await _db.InventoryDb.FirstOrDefaultAsync(i => i.Id == id);
        }

        public IQueryable<InventoryDb> GetAll()
        {
            return _db.InventoryDb;
        }

        public async Task<InventoryDb> Update(InventoryDb item)
        {
            _db.InventoryDb.Update(item);
            await _db.SaveChangesAsync();
            return item;
        }
    }
}
