using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJApplication.ModelsLayer.Models
{
    public class ApplicationModel:IdentityUser
    {
        [Key]
        int Id { get; set; }
        [Required(ErrorMessage ="Name is required")]
        [DisplayName("Name")]
        public string Name { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [ValidateNever]
        public CompanyModel? Company { get; set; }
        public int? EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        [ValidateNever]
        public EmployeeModel? Employee { get; set; }

        [NotMapped]
        public string? tempRole { get; set; }
        [NotMapped]
        public int? tempId { get; set; }
    }
}
