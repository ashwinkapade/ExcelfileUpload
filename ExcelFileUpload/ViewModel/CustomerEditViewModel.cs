using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExcelFileUpload.ViewModel
{
    public class CustomerEditViewModel
    {
        public int sr { get; set; }
        
        [Required(ErrorMessage = "Company Name is Required"), StringLength(500, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z\s]+$"), Display(Name = "Name")]
        public string CompanyName { get; set; }


        [Required(ErrorMessage = "GSTIN is Required")]
        [RegularExpression(@"\d{2}[A-Z]{5}\d{4}[A-Z]{1}[A-Z\d]{1}[Z]{1}[A-Z\d]{1}"), Display(Name = "GSTIN")]
        public string GSTIN { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }
        [Display(Name = "Turn over Amount:")]
        [Required(ErrorMessage = "TurnoveAmount is required.")]
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})$", ErrorMessage = "Valid Decimal number with maximum 2 decimal places.")]
        public string TurnoveAmount { get; set; }

        [Required(ErrorMessage = "Email Id is Required")]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
        ErrorMessage = "Please enter correct email address")]
        public string ContactEmail { get; set; }
        [Required(ErrorMessage = "Mobile No is Required")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string ContactNumber { get; set; }

    }
}