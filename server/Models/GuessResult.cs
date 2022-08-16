using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace ReCardle.Models
{
    //[JsonConverter(typeof(StringEnumConverter))]
    public enum GuessResult
    {
        //[EnumMember(Value = "UNATTEMPTED")]
        UNATTEMPTED,
        //[EnumMember(Value = "INCORRECT")]
        INCORRECT,
        //[EnumMember(Value = "CORRECT_MAKE")]
        PARTIAL,
        //[EnumMember(Value = "CORRECT")]
        CORRECT,
    }
}
