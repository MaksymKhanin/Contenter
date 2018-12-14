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
    public class AudioRepository : IRepository<Audio>
    {
        private ApplicationDbContext db;

        public AudioRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public async Task<IEnumerable<Audio>> GetItemsListAsync(int id)
        {
            return await db.Audios.ToListAsync();
        }

        public async Task<IEnumerable<Audio>> GetItemsListAsync()
        {
            return await db.Audios.ToListAsync();
        }

        public async Task<Audio> GetItemAsync(int id)
        {
            return await db.Audios.FindAsync(id);
        }

        public async Task CreateAsync(Audio audio)
        {
            db.Audios.Add(audio);
        }

        public async Task UpdateAsync(Audio audio)
        {
            db.Entry(audio).State = EntityState.Modified;
        }

        public async Task DeleteAsync(int id)
        {
            Audio audio = await db.Audios.FindAsync(id);
            if (audio != null)
                db.Audios.Remove(audio);
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