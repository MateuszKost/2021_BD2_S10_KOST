using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Utilities.HashGenerator;
using System;
using System.Collections.Generic;

namespace SmartCollection.Tests
{
    [TestClass]
    public class HashGeneratorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HashGeneratorArgumentNull()
        {
            Sha1HashGenerator tagCreator = new Sha1HashGenerator();

            byte[] arr = null;
            
            tagCreator.GetHash(arr);

         }
    }
}
