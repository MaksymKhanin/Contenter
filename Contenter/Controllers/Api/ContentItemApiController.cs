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
    public class ContentItemApiController : RootController
    {
        private readonly IRepository<ContentItem> db1;

        public ContentItemApiController(IRepository<ContentItem> r1)
        {
            db1 = r1;
        }
        public ContentItemApiController()
        {

        }

        protected virtual ResponseMessage MakeErrorBlock(string code, string desc)
        {
            return new ResponseMessage { State = "err", Code = code, Desc = desc };
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetContentItemsAsync()
        {
            var errorBlock = new ResponseMessage();
            try
            {
                var contentItemsList = await db1.GetItemsListAsync();
                return Ok(contentItemsList);
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось получить список статей");
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpGet]
        public async Task<IHttpActionResult> GetContentItemAsync(int id)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                ContentItem contentItem = await db1.GetItemAsync(id);
                return Ok(contentItem);
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось найти статью");
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpPost]
        public async Task<IHttpActionResult> PostContentItemAsync([FromBody]ContentItem contentItem)
        {

            var errorBlock = new ResponseMessage();
            try
            {
                if (contentItem != null && ModelState.IsValid)
                {
                    await db1.CreateAsync(contentItem);
                    await db1.SaveAsync();
                    return Ok();
                }
                else
                {
                    errorBlock = MakeErrorBlock("CLO001", "Не удалось добавить статью");
                    return MakeCustomResponse(400, errorBlock);
                }
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось добавить статью");
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpPut]
        public async Task<IHttpActionResult> PutContentItemAsync([FromBody]ContentItem contentItem)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                if (contentItem != null && ModelState.IsValid)
                {
                    if (contentItem.Id != 0)
                    {
                        await db1.UpdateAsync(contentItem);

                        await db1.SaveAsync();
                        return Ok();
                    }
                    else
                    {
                        errorBlock = MakeErrorBlock("CLO001", "Не удалось изменить статью");
                        return MakeCustomResponse(400, errorBlock);
                    }

                }
                else
                {
                    errorBlock = MakeErrorBlock("CLO001", "Не удалось изменить статью");
                    return MakeCustomResponse(400, errorBlock);
                }
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось изменить статью");
                return MakeCustomResponse(400, errorBlock);

            }

        }
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteContentItemAsync(ContentItem contentItem)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                await db1.DeleteAsync(contentItem.Id);
                await db1.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось удалить статью");
                return MakeCustomResponse(400, errorBlock);

            }

        }

        protected override void Dispose(bool disposing)
        {
            if (db1 != null)
            {
                db1.Dispose();
                base.Dispose(disposing);
            }
        }
    }
}