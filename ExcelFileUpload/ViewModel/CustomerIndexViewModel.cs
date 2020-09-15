using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExcelFileUpload.ViewModel
{
    public class CustomerIndexViewModel
    {
         
        public int sr { get; set; }

        public string CompanyName { get; set; }
        public string GSTIN { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TurnoveAmount { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }

        //sr, CompanyName, GSTIN, StartDate, EndDate, TurnoveAmount, ContactEmail, ContactNumber
    }
}