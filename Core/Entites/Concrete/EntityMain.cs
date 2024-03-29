﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Entites.Concrete
{
    public class EntityMain
    {
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [DataType(DataType.DateTime)]
        public DateTime? CreatedDate { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }

        [DefaultValue(false)]
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public virtual User CreatedUser { get; set; }

        [ForeignKey(nameof(UpdatedBy))]
        public virtual User UpdatedUser { get; set; }
    }
}
