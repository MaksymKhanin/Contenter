using Contenter.Infrastructure.Repository.DI.Abstract;
using Contenter.Models;
using Contenter.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;


namespace Contenter.Controllers.Api
{

    public class VideoApiController : RootController
    {

        private readonly IRepository<Video> db1;

        public VideoApiController(IRepository<Video> r1)
        {
            db1 = r1;
        }
        public VideoApiController()
        {

        }

        protected virtual ResponseMessage MakeErrorBlock(string code, string desc)
        {
            return new ResponseMessage { State = "err", Code = code, Desc = desc };
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetVideosAsync()
        {
            var errorBlock = new ResponseMessage();
            try
            {
                var videosList = await db1.GetItemsListAsync();
                List<SiteModel> models = new List<SiteModel>();
                foreach (var item in videosList)
                {
                    models.Add(Site.GetModelSite(item.Link));
                }

                return Ok(models);
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось получить список видосов");
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpGet]
        public async Task<IHttpActionResult> GetVideoAsync(int id)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                Video video = await db1.GetItemAsync(id);
                return Ok(video);
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось найти видео");
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpPost]
        public async Task<IHttpActionResult> PostVideoAsync([FromBody]Video video)
        {

            var errorBlock = new ResponseMessage();
            try
            {
                if (video != null && ModelState.IsValid)
                {
                    await db1.CreateAsync(video);
                    await db1.SaveAsync();
                    return Ok(Site.GetModelSite(video.Link));
                }
                else
                {
                    errorBlock = MakeErrorBlock("CLO001", "Не удалось добавить видео");
                    return MakeCustomResponse(400, errorBlock);
                }
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось добавить видео");
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpPut]
        public async Task<IHttpActionResult> PutVideoAsync([FromBody]Video video)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                if (video != null && ModelState.IsValid)
                {
                    if (video.Id != 0)
                    {
                        await db1.UpdateAsync(video);

                        await db1.SaveAsync();
                        return Ok();
                    }
                    else
                    {
                        errorBlock = MakeErrorBlock("CLO001", "Не удалось изменить видео");
                        return MakeCustomResponse(400, errorBlock);
                    }
                    
                }
                else
                {
                    errorBlock = MakeErrorBlock("CLO001", "Не удалось изменить видео");
                    return MakeCustomResponse(400, errorBlock);
                }
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось изменить видео");
                return MakeCustomResponse(400, errorBlock);

            }

        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteVideoAsync(Video video)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                await db1.DeleteAsync(video.Id);
                await db1.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось удалить видео");
                return MakeCustomResponse(400, errorBlock);

            }
            
        }
        protected override void Dispose(bool disposing)
        {
            db1.Dispose();
            base.Dispose(disposing);
        }
    }
}