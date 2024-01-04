using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
