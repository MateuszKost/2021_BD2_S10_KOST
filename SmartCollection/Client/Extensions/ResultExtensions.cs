﻿using System.Threading.Tasks;
using SmartCollection.Models.ViewModels;
using System.Net.Http;
using System.Net.Http.Json;

namespace SmartCollection.Client.Extensions
{
    public static class ResultExtensions
    {
        public static async Task<Result> ToResult(this Task<HttpResponseMessage> responseTask)
        {
            var response = await responseTask;

            if (!response.IsSuccessStatusCode)
            {
                var errors = await response.Content.ReadFromJsonAsync<string[]>();

                return Result.Failure(errors);
            }

            return Result.Success;
        }
    }
}
