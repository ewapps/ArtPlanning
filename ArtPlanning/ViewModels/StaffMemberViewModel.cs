using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtPlanning.ViewModels
{
    public class StaffMemberViewModel
    {
        public string ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Color { get; set; }
        public string Trigram { get; set; }
    }
}