using static ReCardle.Models.GuessResult;

namespace ReCardle.Models
{
    /// <summary> 
    /// Represents a single guess and its result. 
    /// 
    /// This class is designed to be created ahead of time, and indicates if this guess has not yet been made.
    /// </summary>
    public class Guess
    {
        /// <summary> The result of this guess </summary>
        public GuessResult Result { get; set; } = UNATTEMPTED;

        /// <summary> The actual guess that the user entered </summary>
        public string Answer { get; set; }

        /// <summary> Whether this guess has been attempted </summary>
        public bool IsAttempted => Result != UNATTEMPTED;

        /// <summary> Used for identifying the result of this guess when sharing the user's score</summary>
        public string Emoji => Result switch
        {
            UNATTEMPTED => "➖",
            INCORRECT => "❌",
            PARTIAL => "💡",
            CORRECT => "🏎",
            _ => "➖",
        };

        /// <summary> String representation of this guess </summary>
        public override string ToString() => $"{Result}: {Answer}";
    }
}
