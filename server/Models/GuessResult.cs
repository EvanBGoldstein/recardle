using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ReCardle.Models
{
    /// <summary> Represents the result of a single guess </summary>
    public enum GuessResult
    {
        /// <summary> The user has not attempted this guess yet </summary>
        UNATTEMPTED,
        /// <summary> The user guessed incorrectly </summary>
        INCORRECT,
        /// <summary> The user got the correct make </summary>
        PARTIAL,
        /// <summary> The user guessed correctly, including both make and model </summary>
        CORRECT,
    }
}
