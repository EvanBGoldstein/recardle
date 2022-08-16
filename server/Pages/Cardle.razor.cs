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
        /// <summary>The url of today's data.json </summary>
        public string DataUrl => $"https://cardle.uk/assets/{Date}/data.json?v=4";

        /// <summary> Today's date in the correct format</summary>
        public string Date => DateTime.Today.ToString("yyyy-MM-dd");

        /// <summary> Whether the "tag" (descriptive game text) should be displayed, based on the game</summary>
        public bool ShowTag => GameStatus != INCOMPLETE;

        /// <summary> Whether the game has ended </summary>
        public bool GameEnded => State?.GameEnded ?? false;

        /// <summary> The status of the current game</summary>
        public GameStatus GameStatus => State?.Result ?? INCOMPLETE;

        /// <summary>The state of the current game </summary>
        public GameState State { get; private set; }

        /// <summary>The currently displayed image's information</summary>
        public CardleImageInfo CurrentImage => TodaysData?.Images[State.ShowImage];

        /// <summary> The user's current guess (bound to guess text box) </summary>
        public string CurrentGuess { get; set; }

        /// <summary> The list of models the user has guessed so far during this game </summary>
        public IEnumerable<Guess> GuessList => State?.GuessList() ?? Enumerable.Empty<Guess>();

        /// <summary> The data for today's game </summary>
        public CardleGame TodaysData { get; private set; }

        /// <summary> The text to display in the "Answer" UI element </summary>
        public string Answer => GameStatus switch
        {
            WON or LOST => TodaysData?.Answer ?? "",
            MAKE => TodaysData?.AcceptedMake ?? "",
            _ => ""
        };

        /// <summary> The text to display in the "tag" UI element </summary>
        public string ResultTag => GameStatus switch
        {
            MAKE => "You got the Make, it's a...",
            WON => SuccessTag,
            LOST => "Better Luck Tomorrow! It was the...",
            _ => ""
        };

        /// <summary> The success phrase to display, given the number of tries it took  </summary>
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

        /// <summary> Grants access to the browser's localStorage </summary>
        [Inject]
        ILocalStorageService localStorage { get; set; }

        /// <summary> Get the style for the button representing a given guess </summary>
        /// <param name="i"> The guess number the button represents </param>
        /// <returns>A <see cref="ButtonStyle"/> for the button representing guess <paramref name="i"/></returns>
        public ButtonStyle GetColor(int i) => (State?.Guesses.GetGuess(i).Result ?? null) switch
        {
            INCORRECT => ButtonStyle.Danger,
            PARTIAL => ButtonStyle.Warning,
            CORRECT => ButtonStyle.Success,
            _ => ButtonStyle.Light,
        };

        /// <summary> Initialize the page (load the latest stored game data/create a new game) </summary>
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

        /// <summary>
        /// Set the displayed image to the image at index <paramref name="i"/> (if that image has been unlocked)
        /// </summary>
        /// <param name="i"> The index of the image to display</param>
        public async Task LoadImage(int i)
        {
            if (GameEnded || i <= State.Guesses.AttemptedCount)
                State.ShowImage = i;
        }

        /// <summary> Submit the text in the answer text box as a guess and validate it </summary>
        public async Task MakeGuess()
        {
            bool correct = TodaysData.ValidateGuess(CurrentGuess);
            bool correctMake = TodaysData.ValidateMake(CurrentGuess);
            var result = correct ? CORRECT : correctMake ? PARTIAL : INCORRECT;
            State.SetCurrentGuessResult(result, CurrentGuess);
            CurrentGuess = "";
            await SaveGameState();
        }

        /// <summary> Skip the current guess </summary>
        public async Task SkipGuess()
        {
            State.SetCurrentGuessResult(INCORRECT, CurrentGuess);
            CurrentGuess = "";
            await SaveGameState();
        }

        /// <summary> Load the last stored game state from the browser's local storage </summary>
        public async Task LoadGameState()
        {
            try
            {
                State = await localStorage.GetItemAsync<GameState>("state") ?? new();
            }
            catch { State = new(); }
        }

        /// <summary> Save the current game to the browser's local storage </summary>
        public async Task SaveGameState() => await localStorage.SetItemAsync("state", State);

        private record ShareObj(string title, string text, string url = "https://recardle.icarusnet.io");
        private object[] ScoreShare => new[] { new ShareObj($"ReCardle | A Daily Car Quiz", $"My recardle score for {Date}: {State.ScoreShare}") };

        /// <summary> Share the user's score via the javascript navigator.share() method </summary>
        public async Task ShareScore() => await JSRuntime.InvokeAsync<object>("navigator.share", ScoreShare);
    }
}
