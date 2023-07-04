using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Viho.Models
{
    public class LoginViewModel
    {
        [Required]
        public string UEmail { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string UPass { get; set; }

        


    }
}
