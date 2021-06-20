using SmartCollection.DataAccess.RepositoryPattern;
using SmartCollection.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.Utilities.TagManagement.TagSender
{
    public class TagSender : ITagSender
    {
        IUnitOfWork _unitOfWork;
        public TagSender(IUnitOfWork unitOfWork)
            => _unitOfWork = unitOfWork;


        private void AddAsync(string tag)
        {
            if (_unitOfWork.Tags.Find(p => p.Name == tag).FirstOrDefault() == null)
            {
                _unitOfWork.Tags.AddAsync(new Tag() { Name = tag });
                _unitOfWork.Save();
            }
        }
        public void AddTagsAsync(IEnumerable<string> tagList)
        {
            foreach (var tag in tagList)
                AddAsync(tag);
        }
    }
}
