using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJApplication.ModelsLayer.Models
{
    public class CompanyModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Email {  get; set; }
        public string PhoneNumber {  get; set; }
        public string PresentAddress {  get; set; }
        public string ParmanentAddress {  get; set; }
        public string PostalCode { get; set; }
        [DataType(DataType.Date)]
        public DateTime EstablishedDate { get; set; }
        [NotMapped]
        [Required(ErrorMessage ="Password is required")]
        [StringLength(100,MinimumLength =8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character")]
        public string? Password { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Please confirm your password")]
        [Compare("Password",ErrorMessage = "Please confirm your password")]
        public string? ConfirmPassword { get; set; }

    }
}
