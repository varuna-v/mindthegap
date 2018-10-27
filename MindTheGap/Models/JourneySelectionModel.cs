using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MindTheGap.Models
{
    public class JourneySelectionModel
    {
        [Display(Name = "From Station")]
        public string FromStationCode { get; set; }
        [Display(Name = "To Station")]
        public string ToStationCode { get; set; }
    }
}