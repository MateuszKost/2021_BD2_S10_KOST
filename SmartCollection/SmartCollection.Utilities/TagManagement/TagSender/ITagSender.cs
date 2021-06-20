using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.TagManagement.TagSender
{
    public interface ITagSender
    {
        public void AddTagsAsync(IEnumerable<string> tagList);
    }
}
