using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radzen;
using Radzen.Blazor;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using ReCardle.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;

namespace ReCardle.Pages
{
    public partial class CardleComponent
    {
        //public string ImagePath => 
        public string DataUrl => $"https://cardle.uk/assets/{Date}/data.json?v=4";

        public string Date => DateTime.Today.ToString("yyyy-MM-dd");//2022 - 08 - 11

        public State State { get; private set; }
        public Image CurrentImage => TodaysData.Images[State?.Guess ?? 1];
        public CardleData TodaysData { get; private set; }

        [Inject]
        ILocalStorageService localStorage { get; set; }

        public async Task Initialize()
        {
            State = await localStorage.GetItemAsync<State>("state");
            if (State.Date != DateTime.Today)
                State.Reset();
            var req = await new HttpClient().GetAsync(DataUrl);
            var json = await req.Content.ReadAsStringAsync();
            TodaysData = JsonConvert.DeserializeObject<CardleData>(json);
        }

        public async Task WriteCookieAsync(string name, string value, int days)
        {
            var test = await JSRuntime.InvokeAsync<string>("blazorExtensions.WriteCookie", name, value, days);
        }

    }
}
