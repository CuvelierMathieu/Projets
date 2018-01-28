using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TP_NORTHWND
{
    public class Cryptage
    {
        //Cette méthode va permettre de stocker les mots de passe dans la base de données de
        //manière sécurisée.
        //Le principe est simple : un même string passé à travers cette méthode donnera toujours
        //le même résultat, et chaque résultat ne provient que d'un seul et unique string,
        //permettant ainsi la vérification suivante : si le mot de passe rentré, une fois crypté,
        //correspond au mot de passe crypté stocké, alors les deux, non cryptés, sont identiques
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
