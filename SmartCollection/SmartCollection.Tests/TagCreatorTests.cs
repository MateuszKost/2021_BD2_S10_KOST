using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Utilities.TagManagement.TagCreator;
using System.Collections.Generic;

namespace SmartCollection.Tests
{
    [TestClass]
    public class TagCreatorTests
    {
        [TestMethod]
        public void TagCreatorTest()
        {
            TagCreator tagCreator = new TagCreator();

            List<string> result = new List<string>(tagCreator.CreateTagList("#dog#cat#student"));

            List<string> expected = new List<string>
            {"DOG","CAT","STUDENT" };
           
           // Assert.AreEqual(expected,result);
          for(int i = 0;i<expected.Count;i++)
                Assert.AreEqual(expected[i], result[i]);
        }
    }
}
