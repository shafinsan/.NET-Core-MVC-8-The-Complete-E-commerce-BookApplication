using EJApplication.ModelsLayer.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJApplication.ModelsLayer.Utility.Wrapper
{
    public class ApplicationUserAndRole
    {
        public ApplicationModel myModel { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>Role {  get; set; }
    }
}
