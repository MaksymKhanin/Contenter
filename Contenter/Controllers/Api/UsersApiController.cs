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

    public class UsersApiController : RootController
    {

        private readonly IRepository<User> db1;

        public UsersApiController(IRepository<User> r1)
        {
            db1 = r1;
        }
        public UsersApiController()
        {

        }

        protected virtual ResponseMessage MakeErrorBlock(string code, string desc)
        {
            return new ResponseMessage { State = "err", Code = code, Desc = desc };
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetUsersAsync()
        {
            var errorBlock = new ResponseMessage();
            try
            {
                var usersList = await db1.GetItemsListAsync();
                return Ok(usersList);
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось получить список юзеров");
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpGet]
        public async Task<IHttpActionResult> GetUserAsync(int id)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                User user = await db1.GetItemAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось найти юзера");
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpPost]
        public async Task<IHttpActionResult> PostUserAsync([FromBody]User user)
        {

            var errorBlock = new ResponseMessage();
            try
            {
                if (user != null && ModelState.IsValid)
                {
                    await db1.CreateAsync(user);
                    await db1.SaveAsync();
                    return Ok();
                }
                else
                {
                    errorBlock = MakeErrorBlock("CLO001", "Не удалось добавить юзера");
                    return MakeCustomResponse(400, errorBlock);
                }
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось добавить юзера");
                return MakeCustomResponse(400, errorBlock);
            }

        }

        [HttpPut]
        public async Task<IHttpActionResult> PutUserAsync([FromBody]User user)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                if (user != null && ModelState.IsValid)
                {
                    if (user.UserId != 0)
                    {
                        await db1.UpdateAsync(user);
                        await db1.SaveAsync();
                        return Ok();
                    }
                    else
                    {
                        errorBlock = MakeErrorBlock("CLO001", "Не удалось изменить юзера");
                        return MakeCustomResponse(400, errorBlock);
                    }

                }
                else
                {
                    errorBlock = MakeErrorBlock("CLO001", "Не удалось изменить юзера");
                    return MakeCustomResponse(400, errorBlock);
                }
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось изменить юзера");
                return MakeCustomResponse(400, errorBlock);

            }

        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteUserAsync(User user)
        {
            var errorBlock = new ResponseMessage();
            try
            {
                await db1.DeleteAsync(user.UserId);
                await db1.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                errorBlock = MakeErrorBlock("CLO001", "Не удалось удалить юзера");
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