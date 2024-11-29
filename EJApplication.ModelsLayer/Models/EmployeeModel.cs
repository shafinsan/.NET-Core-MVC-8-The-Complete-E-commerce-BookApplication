using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJApplication.ModelsLayer.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PresentAddress { get; set; }
        public string ParmanentAddress { get; set; }
        public string PostalCode { get; set; }
        public DateTime JoingDate { get; set; }
    }
}
