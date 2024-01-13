using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required] //For the Razor Page
        public string? Name { get; set; }
        [Required]
        [DisplayName("Display Order")] //Lables on form
       public int DisplayOrder { get; set; }
    }
}
