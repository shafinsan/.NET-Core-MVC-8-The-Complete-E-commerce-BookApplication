using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJApplication.ModelsLayer.Models
{
    public class ShopingCartModel
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        [ValidateNever]
        public BookModel? Book { get; set; }
        [Range(1, 100, ErrorMessage = "The count must be between 1 and 100.")]
        public int Count { get; set; }
        public string ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        [ValidateNever]
        public ApplicationModel? ApplicationUser { get; set; }
        [NotMapped]
        public double? Price {  get; set; }
    }
}
