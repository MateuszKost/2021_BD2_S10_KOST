﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.TagManagement.TagCreator
{
    public interface ITagCreator
    {
        public IEnumerable<string> CreateTagList(string tags);
    }
}