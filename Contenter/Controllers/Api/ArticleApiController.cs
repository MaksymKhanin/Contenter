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
    public class ArticleApiController : RootController
    {
        private readonly IRepository<Article> db1;

        public ArticleApiController(IRepository<Article> r1)
        {
            db1 = r1;
        }
        public ArticleApiController()
        {

        }

        protected virtual ResponseMessage MakeErrorBlock(string code, string desc)
        {
            return new ResponseMessage { State = "err", Code = code, Desc = desc };
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetArticlesAsync()
        {
            var errorBlock = new ResponseMessage();
            try
            {
                var articlesList = await db1.GetItemsListAsync();
                var modelSiteList = new List<SiteModel>();
                foreach (var item in articlesList)
                {
                    modelSiteList.Add(GetModelSite(item.Link));
                }
                return Ok(modelSiteList);
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось получить список статей");
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpGet]
        public async Task<IHttpActionResult> GetArticleAsync(int id)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                Article article = await db1.GetItemAsync(id);
                return Ok(article);
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось найти статью");
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpPost]
        public async Task<IHttpActionResult> PostArticleAsync([FromBody]Article article)
        {

            var errorBlock = new ResponseMessage();
            try
            {
                if (article != null && ModelState.IsValid)
                {
                    await db1.CreateAsync(article);
                    await db1.SaveAsync();
                    return Ok(GetModelSite(article.Link));
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
        public async Task<IHttpActionResult> PutArticleAsync([FromBody]Article article)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                if (article != null && ModelState.IsValid)
                {
                    if (article.Id != 0)
                    {
                        await db1.UpdateAsync(article);

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
        public async Task<IHttpActionResult> DeleteArticleAsync(Article article)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                await db1.DeleteAsync(article.Id);
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