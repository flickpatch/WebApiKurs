using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiKurs.Connection;
using WebApiKurs.Entities;
using WebApiKurs.Token;

namespace WebApiKurs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AutorizeController : ControllerBase
    {
        EfModel model;
        public AutorizeController(EfModel efmodel)
        {
            model = efmodel;
        }
        [HttpPost("/token")]
        public ActionResult<object> Token(string login, string pass)
        {
            var identy = GetIdentity(login, pass);
            if (identy == null)
                return BadRequest();
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                audience: TokenGenerate.AUDINCE, 
                issuer: TokenGenerate.ISSUER, 
                notBefore: now, 
                claims: identy.Claims, 
                expires: now.Add(TimeSpan.FromMinutes(TokenGenerate.LifeTime)),
                signingCredentials: new SigningCredentials(TokenGenerate.GenerateKey(), SecurityAlgorithms.HmacSha256));
            var encodedJWT = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                access_token = encodedJWT,
                username = identy.Name
            };
            return response;

        }
        [NonAction]
        private ClaimsIdentity GetIdentity(string login, string pass)
        {
            User user = model.Users.FirstOrDefault(u => u.Login == login && u.Pass == pass);
            if(user!=null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                    new Claim("Id", user.Id.ToString())
                };
                ClaimsIdentity claimsIdenty = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdenty;
            }
            return null;

        }
    }
}
