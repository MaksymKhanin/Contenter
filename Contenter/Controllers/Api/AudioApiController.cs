using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Contenter.Infrastructure.Repository.DI.Abstract;
using Contenter.Models;

namespace Contenter.Controllers.Api
{
    public class AudioApiController : RootController
    {
        private readonly IRepository<Audio> db1;

        public AudioApiController(IRepository<Audio> r1)
        {
            db1 = r1;
        }
        public AudioApiController()
        {

        }

        protected virtual ResponseMessage MakeErrorBlock(string code, string desc)
        {
            return new ResponseMessage { State = "err", Code = code, Desc = desc };
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAudiosAsync()
        {
            var errorBlock = new ResponseMessage();
            try
            {
                var audiosList = await db1.GetItemsListAsync();
                return Ok(audiosList);
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось получить список песен");
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAudioAsync(int id)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                Audio audio = await db1.GetItemAsync(id);
                return Ok(audio);
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось найти песню");
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpPost]
        public async Task<IHttpActionResult> PostAudioAsync([FromBody]Audio audio)
        {

            var errorBlock = new ResponseMessage();
            try
            {
                if (audio != null && ModelState.IsValid)
                {
                    await db1.CreateAsync(audio);
                    await db1.SaveAsync();
                    return Ok();
                }
                else
                {
                    errorBlock = MakeErrorBlock("CLO001", "Не удалось добавить песню");
                    return MakeCustomResponse(400, errorBlock);
                }
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось добавить песню");
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpPut]
        public async Task<IHttpActionResult> PutAudioAsync([FromBody]Audio audio)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                if (audio != null && ModelState.IsValid)
                {
                    if (audio.Id != 0)
                    {
                        await db1.UpdateAsync(audio);

                        await db1.SaveAsync();
                        return Ok();
                    }
                    else
                    {
                        errorBlock = MakeErrorBlock("CLO001", "Не удалось изменить песню");
                        return MakeCustomResponse(400, errorBlock);
                    }

                }
                else
                {
                    errorBlock = MakeErrorBlock("CLO001", "Не удалось изменить песню");
                    return MakeCustomResponse(400, errorBlock);
                }
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось изменить песню");
                return MakeCustomResponse(400, errorBlock);

            }

        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAudioAsync(Audio audio)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                await db1.DeleteAsync(audio.Id);
                await db1.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось удалить песню");
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
