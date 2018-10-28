using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MindTheGap.Models
{
    public class JourneySelectionModel
    {
        [Display(Name = "Where does your journey start?")]
        public string FromStationCode { get; set; }
        [Display(Name = "Where does your journey end?")]
        public string ToStationCode { get; set; }
    }
}