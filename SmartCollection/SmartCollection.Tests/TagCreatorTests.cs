//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using SmartCollection.DataAccess.RepositoryPattern;
//using System;
//using System.Collections.Generic;

//namespace SmartCollection.Tests
//{
//    [TestClass]
//    public class TagCreatorTests
//    {
//        [TestMethod]
//        public void TagCreatorExpectedResult()
//        {
//            TagCreator tagCreator = new TagCreator();

//            List<string> result = new List<string>(tagCreator.CreateTagList("  #dog#cat  #student   "));

//            List<string> expected = new List<string>
//            {"DOG","CAT","STUDENT" };

//            for (int i = 0; i < expected.Count; i++)
//                Assert.AreEqual(expected[i], result[i]);
//        }
//        [TestMethod]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void TagCreatorNullString()
//        {
//           TagCreator tagCreator = new TagCreator();

//           tagCreator.CreateTagList(null);
//        }
//    }
//}
