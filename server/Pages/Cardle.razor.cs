using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radzen;
using System.Net.Http;
using ReCardle.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using static ReCardle.Models.GameStatus;
using static ReCardle.Models.GuessResult;
namespace ReCardle.Pages
{
    public partial class CardleComponent
    {
        public string DataUrl => $"https://cardle.uk/assets/{Date}/data.json?v=4";

        public string Date => DateTime.Today.ToString("yyyy-MM-dd");

        public bool ShowTag => State?.Result.HasValue ?? false;

        public bool GameEnded => State?.GameEnded ?? false;

        public GameStatus GameStatus => State?.Result ?? INCOMPLETE;

        public GameState State { get; private set; }
        public Image CurrentImage => TodaysData?.Images[State.ShowImage];
        public string CurrentGuess { get; set; }

        public IEnumerable<Guess> GuessList => State?.GuessList() ?? Enumerable.Empty<Guess>();

        public CardleGame TodaysData { get; private set; }

        public string Answer => GameStatus switch
        {
            WON or LOST => TodaysData?.Answer ?? "",
            MAKE => TodaysData?.AcceptedMake ?? "",
            _ => ""
        };

        public string ResultTag => GameStatus switch
        {
            MAKE => "You got the Make, it's a...",
            WON => SuccessTag,
            LOST => "Better Luck Tomorrow! It was the...",
            _ => ""
        };

        public string SuccessTag => (State?.Guesses.AttemptedCount ?? 0) switch
        {
            1 => "Impressive! You're obviously quite familiar with the...",
            2 => "That was quick! It's the...",
            3 => "Alright, you got it! It's the...",
            4 => "You figured it out! It's the...",
            5 => "You knew it from the recognizable piece! It's the...",
            6 => "You knew the car, but not the fine details. It's the...",
            _ => "It's the...",
        };

        [Inject]
        ILocalStorageService localStorage { get; set; }

        public ButtonStyle GetColor(int i) => (State?.Guesses.GetGuess(i).Result ?? null) switch
        {
            INCORRECT => ButtonStyle.Danger,
            PARTIAL => ButtonStyle.Warning,
            CORRECT => ButtonStyle.Success,
            _ => ButtonStyle.Light,
        };

        public async Task Initialize()
        {
            await LoadGameState();
            if (State.Date != DateTime.Today)
                State.Reset();
            else State.ShowDefaultImage();

            var req = await new HttpClient().GetAsync(DataUrl);
            var json = await req.Content.ReadAsStringAsync();
            TodaysData = JsonConvert.DeserializeObject<CardleGame>(json);
            Reload();
        }

        public async Task LoadImage(int i)
        {
            if (GameEnded || i <= State.Guesses.AttemptedCount)
                State.ShowImage = i;
        }

        public async Task MakeGuess()
        {
            bool correct = TodaysData.ValidateGuess(CurrentGuess);
            bool correctMake = TodaysData.ValidateMake(CurrentGuess);
            var result = correct ? CORRECT : correctMake ? PARTIAL : INCORRECT;
            State.SetGuessResult(result, CurrentGuess);
            CurrentGuess = "";
            await SaveGameState();
        }

        public async Task SkipGuess()
        {
            State.SetGuessResult(INCORRECT, CurrentGuess);
            CurrentGuess = "";
            await SaveGameState();
        }

        public async Task LoadGameState()
        {
            try
            {
                State = await localStorage.GetItemAsync<GameState>("state") ?? new();
            }
            catch { State = new(); }
        }

        public async Task SaveGameState() => await localStorage.SetItemAsync("state", State);

        private record ShareObj(string title, string text, string url = "https://recardle.icarusnet.io");
        private object[] ScoreShare => new[] { new ShareObj($"ReCardle | A Daily Car Quiz", $"My recardle score for {Date}: {State.ScoreShare}") };

        public async Task ShareScore() => await JSRuntime.InvokeAsync<object>("navigator.share", ScoreShare);
    }
}
