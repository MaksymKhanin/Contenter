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
    public class ContentItemRepository : IRepository<ContentItem>
    {
        private ApplicationDbContext db;

        public ContentItemRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public async Task<IEnumerable<ContentItem>> GetItemsListAsync(int id)
        {
            return await db.ContentItems.ToListAsync();
        }

        public async Task<IEnumerable<ContentItem>> GetItemsListAsync()
        {
            return await db.ContentItems.ToListAsync();
        }

        public async Task<ContentItem> GetItemAsync(int id)
        {
            return await db.ContentItems.FindAsync(id);
        }

        public async Task CreateAsync(ContentItem contentItem)
        {
            db.ContentItems.Add(contentItem);
        }

        public async Task UpdateAsync(ContentItem contentItem)
        {
            db.Entry(contentItem).State = EntityState.Modified;
        }

        public async Task DeleteAsync(int id)
        {
            ContentItem contentItem = await db.ContentItems.FindAsync(id);
            if (contentItem != null)
                db.ContentItems.Remove(contentItem);
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