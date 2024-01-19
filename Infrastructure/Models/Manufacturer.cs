using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Manufacturer
    {
        [Key]
        public int Id { get; set; }
        [Required] //For the Razor Page
        public string? Name { get; set; }
    }
}
