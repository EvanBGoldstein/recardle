using static ReCardle.Models.GuessResult;

namespace ReCardle.Models
{
    public class Guess
    {
        public GuessResult Result { get; set; } = UNATTEMPTED;
        public string Answer { get; set; } 

        public bool IsUnattempted => Result is GuessResult.UNATTEMPTED;

        public string Emoji => Result switch
        {
            UNATTEMPTED => "➖",
            INCORRECT => "❌",
            PARTIAL => "💡",
            CORRECT => "🏎",
            _ => "➖",
        };

        public override string ToString() => $"{Result}: {Answer}";
    }
}
