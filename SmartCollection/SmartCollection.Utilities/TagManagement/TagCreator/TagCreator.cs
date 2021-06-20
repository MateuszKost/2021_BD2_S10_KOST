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
        IUnitOfWork _unitOfWork;
        public TagCreator(IUnitOfWork unitOfWork)
            => _unitOfWork = unitOfWork;

        private void AddAsync(string tag)
        {
            if (_unitOfWork.Tags.Find(p => p.Name == tag).FirstOrDefault() == null)
            {
                _unitOfWork.Tags.AddAsync(new Tag() { Name = tag });
                _unitOfWork.Save();
            }
        }

        public void AddTagsAsync(string tags)
        {
            string normTags = tags.Trim(' ').ToUpper();

            IEnumerable<string> tagList = normTags.Split('#').ToList();

            foreach (var tag in tagList)
                AddAsync(tag);
        }
    }
}
