namespace ReCardle.Models
{
    /// <summary> Represents the status of a single game </summary>
    public enum GameStatus
    {
        /// <summary> The game has not been completed yet, and the user has not guessed the correct make</summary>
        INCOMPLETE,
        /// <summary> The game is over, and the user did not guess the correct model</summary>
        LOST,
        /// <summary> The game has not been completed yet, but user has guessed the correct make</summary>
        MAKE,
        /// <summary> The game is over, and the user has guessed the correct model </summary>
        WON,
    }
}
