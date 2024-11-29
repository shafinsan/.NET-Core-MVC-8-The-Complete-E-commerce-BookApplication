using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJApplication.ModelsLayer.Models
{
    public class CategoryModel
    {
        [Key]
        [DisplayName("Id")]
        public int Id { get; set; }
        [Required]
        [DisplayName("Name")]
        [MinLength(4, ErrorMessage = "Name Length can not be Smaller then 2"),
            MaxLength(20, ErrorMessage = "Name Length can not be Greater then 10")]
        public string Name { get; set; }
    }
}
