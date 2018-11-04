using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Contenter.Models;
using Contenter.Infrastructure.Repository.DI.Abstract;
using Contenter.Models.Context;

namespace Contenter.Controllers.Api
{
    public class UsersApiController : ApiController
    {
        private readonly IRepository<User> db1;

        public UsersApiController(IRepository<User> r1)
        {
            db1 = r1;
        }
        public UsersApiController()
        {

        }

        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            List<User> users = new List<User>();
            var usersList = db1.GetItemsList();
            foreach (var person in usersList)
            {
                var user = new User
                {
                    UserId = person.UserId,
                    Firstname=person.Firstname,
                    Sirname = person.Sirname,
                    Email = person.Email,
                    PhoneNumber = person.PhoneNumber,
                    Password = person.Password,
                    FollowersNumber = person.FollowersNumber,
                    FollowingNumber = person.FollowingNumber
                    
                };
                users.Add(user);
            }
            return users;
        }

        [HttpGet]
        public User GetCandidate(int id)
        {
            User user = db1.GetItem(id);
            return user;
        }

        [HttpPost]
        public void PostUser(User user)
        {
            db1.Create(user);
            db1.Save();
        }

        [HttpPut]
        public void PutUser(User user)
        {
            if (user != null && ModelState.IsValid)
            {
                if (user.UserId != 0)
                {
                    db1.Update(user);

                    db1.Save();
                }
            }

        }

        [HttpDelete]
        public void DeleteUser(User user)
        {
            db1.Delete(user.UserId);
            db1.Save();
        }
        protected override void Dispose(bool disposing)
        {
            db1.Dispose();
            base.Dispose(disposing);
        }
    }
}
