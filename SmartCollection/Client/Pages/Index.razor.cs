using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Client.Pages
{
    public partial class Index
    {
        string[] jobs = { "Hello there ! ", "If you are here for the first time, just create account !", "If not, just log in and do whatever you want !" };
        string typewriterText;
        Task worker;
        protected override void OnInitialized()
        {
            worker = Typewriter();
        }

        async Task Typewriter()
        {
            var index = 0;
            while (true)
            {
                var textIndex = 1;

                while (textIndex < jobs[index].Length + 1)
                {
                    typewriterText = jobs[index].Substring(0, textIndex);
                    textIndex++;
                    StateHasChanged();
                    await Task.Delay(50);
                };

                StateHasChanged();
                await Task.Delay(500);

                textIndex = jobs[index].Length;

                while (textIndex + 1 > 0)
                {
                    typewriterText = jobs[index].Substring(0, textIndex);
                    textIndex--;
                    StateHasChanged();
                    await Task.Delay(50);
                };

                index++;
                if (index == jobs.Length)
                {
                    index = 0;
                }
            }
        }
    }
}
