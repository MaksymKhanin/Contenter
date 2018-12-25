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
    public class ArticleRepository : IRepository<Article>
    {
        private ApplicationDbContext db;

        public ArticleRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public async Task<IEnumerable<Article>> GetItemsListAsync(int id)
        {
            return await db.Articles.ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetItemsListAsync()
        {
            return await db.Articles.ToListAsync();
        }

        public async Task<Article> GetItemAsync(int id)
        {
            return await db.Articles.FindAsync(id);
        }

        public async Task CreateAsync(Article article)
        {
            db.Articles.Add(article);
        }

        public async Task UpdateAsync(Article article)
        {
            db.Entry(article).State = EntityState.Modified;
        }

        public async Task DeleteAsync(int id)
        {
            Article article = await db.Articles.FindAsync(id);
            if (article != null)
                db.Articles.Remove(article);
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