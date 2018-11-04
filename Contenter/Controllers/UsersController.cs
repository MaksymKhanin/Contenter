using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;
using Contenter.Models;
using Contenter.Infrastructure.Repository.DI.Abstract;
using Contenter.Models.Context;


namespace Contenter.Controllers
{
    public class UsersController : Controller
    {
        IRepository<User> db1;
        public UsersController(IRepository<User> r1)
        {
            db1 = r1;
        }
        public ActionResult Index()
        {

            return View();
        }
    }
}