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
        [HttpDelete("{userid}/{productid}")]
        public async Task<ActionResult<Like>> DeleteLike(int productid, int userid)
        {
            Like like = model.Likes.Where(l => l.ProductId == productid && l.UserID == userid).FirstOrDefault();
            if (like == null)
                return NotFound(null);
            model.Likes.Remove(like);
            await model.SaveChangesAsync();
            return Ok(like);
        }
        [HttpGet("{userid}/{productid}")]
        public async Task<ActionResult<Like>> IsLikeProduct(int userid, int productid)
        {
            Like like = await model.Likes.Where(l => l.ProductId == productid && l.UserID == userid).FirstOrDefaultAsync();
            if (like != null)
                return Ok(like);
            return NotFound(null);
        }
        
        [HttpGet("likebyid/{userid}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetYourLikes(int userid)
        {
            /*var nas = await model.Products.Where(p => p.Likes.Where(l => l.UserID == userid) != null).Include(p=> p.Likes).ToListAsync();
            if (nas == null)
                return null;
            return nas;*/
           
            var mas = await model.Likes.Where(l => l.UserID == userid).Include(l=> l.product).ToListAsync();   
            return  GetProductByLike(mas);
        }
        private List<Product> GetProductByLike(List<Like> likes)
        {
            List<Product> products = new List<Product>();
            foreach (var i in likes)
            {
                products.Add(i.product);
            }
            return products;

        }
        [HttpPost("like/{productid}/{Userid}")]
        public async Task<ActionResult<Like>> LikeProduct(int productid, int Userid)
        {         
            Like like = new Like()
            {
                IsLiked = true,                
                ProductId = productid,                
                UserID = Userid
            };
            model.Likes.Add(like);
            await model.SaveChangesAsync();
            return Ok(like);
        }
      
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
