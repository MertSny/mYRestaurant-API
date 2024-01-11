using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Core.Enums
{
    public class OrderByEnums
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [DefaultValue(CreatedDate)]
        public enum DefaultOrderBy
        {
            CreatedDate,
            Id,
        }
    }
}
