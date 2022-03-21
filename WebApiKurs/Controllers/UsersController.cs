using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiKurs.Connection;
using WebApiKurs.Entities;

namespace WebApiKurs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    { 
        EfModel model;
        public UsersController(EfModel efmodel)
        {
            model = efmodel;
        }
       /* [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await model.Users.ToListAsync();
        }*/
        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            if(user == null)
            {
                return BadRequest();
            }
            model.Users.Add(user);
            await model.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            User user = await model.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
                return NotFound();
            return Ok(user);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            User u = await model.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            if(u!= null)
            {
                model.Users.Remove(u);
                await model.SaveChangesAsync();
                return Ok(u);
            }
            return NotFound(u);
        }
        [HttpPut("{User}")]
        public async Task<ActionResult<User>> ChangeUser(User user)
        {
            if(user == null)
            {
                return BadRequest();  
            }
            if (!model.Users.Any(u => u.Id == user.Id))
                return NotFound();
            model.Users.Update(user);
            await model.SaveChangesAsync();
            return Ok(user);
        }
    }
}
