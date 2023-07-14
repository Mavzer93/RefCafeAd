using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RefCafeAData
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }

        [Display(Name = "Kayıt T.")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Durum")]
        public bool Enabled { get; set; }
    }
}
