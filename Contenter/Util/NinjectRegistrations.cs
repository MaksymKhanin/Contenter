﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using Contenter.Models;
using Contenter.Infrastructure.Repository.DI.Abstract;
using Contenter.Infrastructure.Repository.DI.Implementation;
using Contenter.Models.Context;


namespace Contenter.Util
{
    public class NinjectRegistrations:NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository<User>>().To<UserRepository>();

        }
    }
}