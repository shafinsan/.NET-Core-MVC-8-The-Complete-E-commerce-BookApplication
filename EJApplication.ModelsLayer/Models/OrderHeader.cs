using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJApplication.ModelsLayer.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }
        public string? ApplicationId { get; set; }
        [ValidateNever]
        [ForeignKey("ApplicationId")]
        public ApplicationModel? ApplicationUser { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        public double? TotalOrder { get; set; }



        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrakingNumber { get; set; }
        public string? Carrier { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? PaymentDueDate { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? SessionId { get; set; }




        [Required(ErrorMessage ="Phone is requird")]
        [RegularExpression(@"^(018|017|015|016|019)[0-9]{8}$",ErrorMessage ="Number format wrong")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Name is requird")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Street is requird")]
        public string StreetAddress { get; set; }
        [Required(ErrorMessage = "City is requird")]
        public string City { get; set; }
        [Required(ErrorMessage = "State is requird")]
        public string State { get; set; }
        [Required(ErrorMessage = "PostalCode is requird")]
        public string PostalCode { get; set; }

    }
}
