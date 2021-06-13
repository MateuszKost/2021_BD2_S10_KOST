using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Client.Shared
{
    public partial class ModalComponent
    {
        [Parameter]
        public string ModalTitle { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback<bool> OnClose { get; set; }

        public Task ModalClose()
        {
            return OnClose.InvokeAsync(true);
        }

    }
}
