using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.TagManagement.TagCreator
{
    public class TagCreator : ITagCreator
    {
        public IEnumerable<string> CreateTagList(string tags)
        {
            string normTags = tags.Trim(' ').ToUpper();

            List<string> tagList = normTags.Split('#').ToList();
            
            tagList.Remove("");
            
            return tagList;

        }
    }
}
