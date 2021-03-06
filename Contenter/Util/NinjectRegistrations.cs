﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using Contenter.Models;
using Contenter.Infrastructure.Repository.DI.Abstract;
using Contenter.Infrastructure.Repository.DI.Implementation;


namespace Contenter.Util
{
    public class NinjectRegistrations:NinjectModule
    {
        public override void Load()
        {
            Bind<IRepository<User>>().To<UserRepository>();
            Bind<IRepository<Video>>().To<VideoRepository>();
            Bind<IRepository<Audio>>().To<AudioRepository>();
            Bind<IRepository<Article>>().To<ArticleRepository>();
            Bind<IRepository<ContentItem>>().To<ContentItemRepository>();
        }
    }
}