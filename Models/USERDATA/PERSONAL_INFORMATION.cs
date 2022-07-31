using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMIS_PERSONEL_PROFILE.Models
{
    [MetadataType(typeof(PERSONAL_INFORMATIONMetaData))]
    public partial class PERSONAL_INFORMATION
    {
   
    }
    public class PERSONAL_INFORMATIONMetaData
    {
        [Display(Name = "File Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "File Number field required")]
        [MinLength(4, ErrorMessage = "invalid File Number length")]
        public string FileNo { get; set; }

        [Display(Name = "NIN ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "NIN field required")]
        [MinLength(13,ErrorMessage ="invalid nin length")]
        public string NIN { get; set; }

        [Display(Name = "Security Passcode")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Passcode field required")]
        [MinLength(5, ErrorMessage = "invalid nin length")]
        public string Passcode { get; set; }
    }


}