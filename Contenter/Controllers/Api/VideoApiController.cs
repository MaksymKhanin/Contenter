using Contenter.Infrastructure.Repository.DI.Abstract;
using Contenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Contenter.Controllers.Api
{

    public class VideoApiController : ApiController
    {
        private readonly IRepository<Video> db1;

        public VideoApiController(IRepository<Video> r1)
        {
            db1 = r1;
        }
        public VideoApiController()
        {

        }

        [HttpGet]
        public IEnumerable<Video> GetVideos()
        {
            List<Video> videos = new List<Video>();
            var videosList = db1.GetItemsList();
            foreach (var vidos in videosList)
            {
                var video = new Video
                {
                    Id = vidos.Id,
                    Link = vidos.Link

                };
                videos.Add(video);
            }
            return videos;
        }

        [HttpGet]
        public Video GetVideo(int id)
        {
            Video video = db1.GetItem(id);
            return video;
        }

        [HttpPost]
        public void PostVideo([FromBody]Video video)
        {
            
            db1.Create(video);
            db1.Save();
        }

        [HttpPut]
        public void PutVideo([FromBody]Video video)
        {
            if (video != null && ModelState.IsValid)
            {
                if (video.Id != 0)
                {
                    db1.Update(video);

                    db1.Save();
                }
            }

        }

        [HttpDelete]
        public void DeleteVideo(Video video)
        {
            db1.Delete(video.Id);
            db1.Save();
        }
        protected override void Dispose(bool disposing)
        {
            db1.Dispose();
            base.Dispose(disposing);
        }
    }
}