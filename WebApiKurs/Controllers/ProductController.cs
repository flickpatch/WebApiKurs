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
        [HttpGet("id/{id}")]       
       public async Task<ActionResult<IEnumerable<Product>>> GetProducts(int id)
        {            
            return await model.Products.Where(p=> p.UserId!= id).Include(p=> p.User).ToListAsync();
        }
        [Authorize]
        [HttpGet("{userid}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetYoueProducts(int userid)
        {
            List<Product> products = await model.Products.Where(p => p.UserId == userid).Include(p => p.User).ToListAsync();
            if (products == null)
                return null;
            return products;
        }
        [Authorize]
        [HttpGet("/search/{search}/")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsFromSearch(string search)
        {
            List<Product> products = await model.Products.Include(p => p.User).ToListAsync();
            products = products.Where(p => p.Name.Contains(search) || GetLeveshtain(search, p.Name) <= 3).ToList();
            if (products == null)
                return  null; 
            return products;
        }
        private int GetLeveshtain(string search, string check)
        {
            var n = search.Length + 1;
            var m = check.Length + 1;
            int[,] mat = new int[n, m];
            for (int i = 0; i < n; i++)
            {
                mat[i, 0] = i;
            }
            for (int j = 0; j < m; j++)
            {
                mat[0, j] = j;
            }
            for (int a = 1; a < n; a++)
            {
                for (int b = 1; b < m; b++)
                {
                    int raz = search[a - 1] == check[b - 1] ? 0 : 1;
                    mat[a, b] = Math.Min(Math.Min(mat[a - 1, b] + 1, mat[a, b - 1] + 1), mat[a - 1, b - 1] + raz);
                }
            }
            return mat[n - 1, m - 1];
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
