using SmartCollection.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Models.ViewModels.TagsViewModel
{
    public class TagsViewModel
    {
        public IEnumerable<Tag> Tags { get; set; }
    }
}
