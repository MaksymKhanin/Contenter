using Contenter.Infrastructure.Repository.DI.Abstract;
using Contenter.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace Contenter.Infrastructure.Repository.DI.Implementation
{
    public class UserRepository : IRepository<User>
    {

        private ApplicationDbContext db;

        public UserRepository(ApplicationDbContext context)
        {
            this.db = context;
        }



        public async Task<IEnumerable<User>> GetItemsListAsync(int id)

        {
            return await db.Users.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetItemsListAsync()
        {
            return await db.Users.ToListAsync();
        }

        public async Task<User> GetItemAsync(int id)
        {
            return await db.Users.FindAsync(id);
        }

        public async Task CreateAsync(User user)
        {
            db.Users.Add(user);
        }

        public async Task UpdateAsync(User user)
        {
            db.Entry(user).State = EntityState.Modified;
        }

        public async Task DeleteAsync(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user != null)
                db.Users.Remove(user);
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}