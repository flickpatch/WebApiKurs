using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiKurs.Token
{
    public class TokenGenerate
    {
        public const string ISSUER = "AuthServer";
        public const string AUDINCE = "AucthClient";
        const string KEY = "SDdbsajhk8adbasJASdjsakd!jSDasdkasb@#@jadksaj";
        public const int LifeTime = 30;
        public static SymmetricSecurityKey GenerateKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
