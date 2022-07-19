using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Shop.Models.JWT
{
    public class JWTModel
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        const string KEY = "wwwoootimur!love";   // ключ для шифрации
        public const int LIFETIME = 60;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
