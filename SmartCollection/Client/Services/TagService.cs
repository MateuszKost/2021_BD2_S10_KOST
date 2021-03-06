using SmartCollection.Models.DBModels;
using SmartCollection.Models.ViewModels.TagsViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SmartCollection.Client.Services
{
    public class TagService : ITagService<Tag>
    {
        private readonly HttpClient _httpClient;
        public TagService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Tag>> GetTags(int albumId)
        {
            var tagsViewModel = await _httpClient.GetFromJsonAsync<TagsViewModel>("tags/get/" + albumId);
            return tagsViewModel.Tags ?? null;
        }

        public IEnumerable<string> CreateTagList(string tags)
        {
            if (tags != null)
            {
                string normTags = tags.Replace(" ", "").ToUpper();

                List<string> tagList = normTags.Split('#').ToList();

                tagList.Remove("");

                return tagList;
            }
            throw new ArgumentNullException();
        }
    }
}
