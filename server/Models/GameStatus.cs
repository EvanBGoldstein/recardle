using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ReCardle.Models
{
    //[JsonConverter(typeof(StringEnumConverter))]
    public enum GameStatus
    {
        //[EnumMember(Value = "INCOMPLETE")]
        INCOMPLETE,
        //[EnumMember(Value = "LOST")]
        LOST,
        //[EnumMember(Value = "CORRECT_MAKE")]
        MAKE,
        //[EnumMember(Value = "WON")]
        WON,
    }
}
