using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Contenter.Models.Context;
using Contenter.Models;
using Contenter.Infrastructure.Repository.DI.Abstract;

namespace Contenter.Infrastructure.Repository.DI.Implementation
{
    public class UserRepository: IRepository<User>
    {
        private Context db;

        public UserRepository(Context context)
        {
            this.db = context;
        }
        public IEnumerable<User> GetItemsList(int id)
        {
            return db.Users;
        }

        public IEnumerable<User> GetItemsList()
        {
            return db.Users;
        }

        public User GetItem(int id)
        {
            return db.Users.Find(id);
        }

        public void Create(User user)
        {
            db.Users.Add(user);
        }

        public void Update(User user)
        {
            db.Entry(user).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            User user = db.Users.Find(id);
            if (user != null)
                db.Users.Remove(user);
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