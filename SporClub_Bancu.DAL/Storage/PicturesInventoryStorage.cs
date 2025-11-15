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
    public class PicturesInventoryStorage : IBaseStorage<PicturesInventoryDb>
    {
        private readonly ApplicationDbContext _db;

        public PicturesInventoryStorage(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Add(PicturesInventoryDb item)
        {
            await _db.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(PicturesInventoryDb item)
        {
            _db.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task<PicturesInventoryDb> Get(Guid id)
        {
            return await _db.PicturesInventoryDb.FirstOrDefaultAsync(p => p.Id == id);
        }

        public IQueryable<PicturesInventoryDb> GetAll()
        {
            return _db.PicturesInventoryDb;
        }

        public async Task<PicturesInventoryDb> Update(PicturesInventoryDb item)
        {
            _db.PicturesInventoryDb.Update(item);
            await _db.SaveChangesAsync();
            return item;
        }
    }
}
