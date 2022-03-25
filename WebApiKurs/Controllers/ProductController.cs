using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiKurs.Connection;
using WebApiKurs.Entities;

namespace WebApiKurs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        EfModel model;
        public ProductController(EfModel efmodel)
        {
            model = efmodel;
            if(!model.Users.Any())
            {
                model.Users.Add(new User()
                {
                    Id = 1,
                    Name = "Ilya",
                    SecName = "Agafonov",
                    BirthDate = new DateTime(2003, 07, 16),
                    Email = "AAAAA@mail.ru",
                    Login = "1",
                    Pass = "2",
                    
                   
                });
            }
            if(!model.Products.Any())
            {
                model.Products.Add(new Product()
                {
                    Id = 1,
                    Name= "d", Price=3500,  DateCreate = new DateTime(2003,02,03), Type= "Авто", Description="dsadasdasdasdsadhsahdhjsddhjdshjjhksdjhkdsajkhdsajhkdsajkhdsahjksadjkhd", IsActivity = true, UserId = 1 
                } 

                    );
            }
            model.SaveChanges();
        }

        [Authorize]
        [HttpGet]       
       public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
                throw new Exception("notautorized");
            return await model.Products.Include(p=>p.Photos).ToListAsync();
        }
        [HttpGet("{userid}")]
        public async Task<ActionResult<IEnumerable<Product>>>GetProducts(int userid)
        {
            Product product = await model.Products.Where(p => p.UserId == userid).FirstOrDefaultAsync();
            if (product == null)
                return NotFound();
            return Ok(product);
        }
        
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if(product == null)
            {
                return BadRequest();
            }
            model.Products.Add(product);
            await  model.SaveChangesAsync();
            return Ok(product);
        }
        [HttpPut("{Product}")]
        public async Task<ActionResult<Product>> PutProduct(Product product)
        {
            if(product == null)
            {
                return BadRequest();
            }
            if (!model.Products.Any(p=> p.Id == product.Id))
            {
                return NotFound();
            }
            model.Products.Update(product);
            await model.SaveChangesAsync();
            return Ok(product);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            Product p = model.Products.Where(p => p.Id == id).FirstOrDefault();
            if (p == null)
            {
                return NotFound();
            }
            model.Products.Remove(p);
            await model.SaveChangesAsync();
            return Ok(p);
        }
        [Authorize]
        [HttpPost("{product}")]
        public async Task<ActionResult<Product>> AddUserProduct(Product product, int id)
        {
            if (product == null)
                return BadRequest();
            product.DateCreate = DateTime.Now;
            product.IsActivity = true;
            product.UserId = id;
            product.User = model.Users.Find(id);
            model.Products.Add(product);
             await model.SaveChangesAsync();
            return Ok(product);

        }
    }
}
