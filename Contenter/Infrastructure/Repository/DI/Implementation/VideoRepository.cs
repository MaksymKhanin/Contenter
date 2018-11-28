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
    public class VideoRepository : IRepository<Video>
    {
        
        private ApplicationDbContext db;

        public VideoRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        

        public async Task<IEnumerable<Video>> GetItemsListAsync(int id)
        {
            return await db.Videos.ToListAsync();
        }

        public async Task<IEnumerable<Video>> GetItemsListAsync()
        {
            return await db.Videos.ToListAsync();
        }

        public async Task<Video> GetItemAsync(int id)
        {
            return await db.Videos.FindAsync(id);
        }

        public async Task CreateAsync (Video video)
        {
            db.Videos.Add(video);
        }

        public async Task UpdateAsync(Video video)
        {
            db.Entry(video).State = EntityState.Modified;
        }

        public async Task DeleteAsync(int id)
        {
            Video video = await db.Videos.FindAsync(id);
            if (video != null)
                db.Videos.Remove(video);
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