using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.HashGenerator
{
    public class Sha1HashGenerator : IHashGenerator
    {
        public string GetHash(byte[] file)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(file);
            string hash = BitConverter.ToString(hashBytes);
            //string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            
            return hash;
        }
    }
}
