using Contenter.Infrastructure.Repository.DI.Abstract;
using Contenter.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        public IEnumerable<Video> GetItemsList(int id)
        {
            return db.Videos;
        }

        public IEnumerable<Video> GetItemsList()
        {
            return db.Videos;
        }

        public Video GetItem(int id)
        {
            return db.Videos.Find(id);
        }

        public void Create(Video video)
        {
            db.Videos.Add(video);
        }

        public void Update(Video video)
        {
            db.Entry(video).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Video video = db.Videos.Find(id);
            if (video != null)
                db.Videos.Remove(video);
        }

        public void Save()
        {
            db.SaveChanges();
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