using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.HashGenerator
{
    public interface IHashGenerator
    {
        public string GetHash(byte[] file);
    }
}
