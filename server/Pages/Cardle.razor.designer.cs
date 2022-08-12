using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ReCardle.Pages
{
    public partial class CardleComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            await Initialize();
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            await LoadImage(0);
        }

        protected async System.Threading.Tasks.Task Button3Click(MouseEventArgs args)
        {
            await LoadImage(1);
        }

        protected async System.Threading.Tasks.Task Button4Click(MouseEventArgs args)
        {
            await LoadImage(2);
        }

        protected async System.Threading.Tasks.Task Button5Click(MouseEventArgs args)
        {
            await LoadImage(3);
        }

        protected async System.Threading.Tasks.Task Button6Click(MouseEventArgs args)
        {
            await LoadImage(4);
        }

        protected async System.Threading.Tasks.Task Button7Click(MouseEventArgs args)
        {
            await LoadImage(5);
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            await MakeGuess();
        }

        protected async System.Threading.Tasks.Task Button1Click(MouseEventArgs args)
        {
            await SkipGuess();
        }
    }
}
