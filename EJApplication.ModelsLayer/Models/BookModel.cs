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
    public class BookModel
    {
        public int Id { get; set; }

        // Book title
        [Required]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string Title { get; set; }

        // ISBN number
        [Required]
        [StringLength(20, ErrorMessage = "ISBN cannot be longer than 20 characters.")]
        public string ISBN { get; set; }

        // Book description
        [Required]
        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
        public string Description { get; set; }

        // Book author
        [Required]
        [StringLength(100, ErrorMessage = "Author name cannot be longer than 100 characters.")]
        public string Author { get; set; }

        // Original list price
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "List price must be greater than zero.")]
        public decimal ListPrice { get; set; }

        // Current price for single unit
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal? Price { get; set; }

        // Price for 50 units
        [Range(0.01, double.MaxValue, ErrorMessage = "Price for 50 units must be greater than zero.")]
        public decimal? Price50 { get; set; }

        // Price for 100 units
        [Range(0.01, double.MaxValue, ErrorMessage = "Price for 100 units must be greater than zero.")]
        public decimal? Price100 { get; set; }

        // Foreign key for Category
        [Required]

        public int CategoryId { get; set; }

        // Category navigation property
        [ValidateNever]
        [ForeignKey("CategoryId")]
        public CategoryModel Category { get; set; }

        // Image URL for the book cover
        [ValidateNever]
        public string ImageUrl { get; set; }
    }
}
