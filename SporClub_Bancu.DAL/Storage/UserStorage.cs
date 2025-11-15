using Microsoft.EntityFrameworkCore;
using SportClub_Bancu.Domain.ModelsDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SporClub_Bancu.DAL.Storage
{
    public class UserStorage : IBaseStorage<UserDb>
    {
        private readonly ApplicationDbContext _db;

        public UserStorage(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Add(UserDb item)
        {
            await _db.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(UserDb item)
        {
            _db.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task<UserDb> Get(Guid id)
        {
            return await _db.UsersDb.FirstOrDefaultAsync(u => u.Id == id);
        }

        public IQueryable<UserDb> GetAll()
        {
            return _db.UsersDb;
        }

        public async Task<UserDb> Update(UserDb item)
        {
            _db.UsersDb.Update(item);
            await _db.SaveChangesAsync();
            return item;
        }

    }
}
