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
using System.Drawing;

namespace ReCardle.Pages
{
    public partial class CardleComponent
    {
        //public string ImagePath => 
        public string DataUrl => $"https://cardle.uk/assets/{Date}/data.json?v=4";

        public string Date => DateTime.Today.ToString("yyyy-MM-dd");

        public bool ShowTag => State?.Result.HasValue ?? false;

        public bool GameEnded => (State?.Result ?? -1) is 0 or 2;

        public State State { get; private set; }
        public Image CurrentImage => TodaysData?.Images[State.ShowImage];
        public string CurrentGuess { get; set; }

        public IEnumerable<GuessState> GuessList
            => State?.Guesses.Where(g => !string.IsNullOrEmpty(g?.Guess)) ?? Enumerable.Empty<GuessState>();
        public CardleData TodaysData { get; private set; }

        public string Answer => GameEnded ? TodaysData?.Answer ?? "" : (State?.Result ?? -1) == 1 ? TodaysData.AcceptedMakes[0] : "";

        public string ResultTag =>
                ((State?.Result ?? -1) == 1) ? "You got the Make, it's a..."
                : ((State?.Result ?? -1) == 2) ? (State?.Guess ?? 0) switch
                {
                    1 => "Impressive! You're obviously quite familiar with the...",
                    2 => "That was quick! It's the...",
                    3 => "Alright, you got it! It's the...",
                    4 => "You figured it out! It's the...",
                    5 => "You knew it from the recognizable piece! It's the...",
                    6 => "You knew the car, but not the fine details. It's the...",
                    _ => "It's the...",
                } : "Better Luck Tomorrow! It was the...";

        [Inject]
        ILocalStorageService localStorage { get; set; }

        public ButtonStyle GetColor(int i) => (State?.Guesses[i]?.Result ?? null) switch
        {
            null => ButtonStyle.Light,
            0 => ButtonStyle.Danger,
            1 => ButtonStyle.Warning,
            2 => ButtonStyle.Success,
            _ => throw new NotImplementedException(),
        };

        public async Task Initialize()
        {
            await LoadGameState();
            if (State.Date != DateTime.Today)
                State.Reset();
            var req = await new HttpClient().GetAsync(DataUrl);
            var json = await req.Content.ReadAsStringAsync();
            TodaysData = JsonConvert.DeserializeObject<CardleData>(json);
            Reload();
        }

        public async Task LoadImage(int i)
        {
            if (GameEnded || i <= State.Guess)
                State.ShowImage = i;
        }

        public async Task MakeGuess()
        {
            bool correct = TodaysData.AcceptedAnswers.Any(a => CurrentGuess.Contains(a.Replace("-", "")));
            bool correctMake = TodaysData.AcceptedMakes.Any(a => CurrentGuess.Contains(a.Replace("-", "")));
            int result = correct ? 2 : correctMake ? 1 : 0;
            State.SetGuessResult(result, CurrentGuess);
            CurrentGuess = "";
            //await SaveGameState();
        }

        public async Task SkipGuess()
        {
            State.SetGuessResult(0, CurrentGuess);
            CurrentGuess = "";
            //await SaveGameState();
        }

        public async Task LoadGameState() => State = await localStorage.GetItemAsync<State>("state") ?? new();
        public async Task SaveGameState() => await localStorage.SetItemAsync("state", State);
    }
}
