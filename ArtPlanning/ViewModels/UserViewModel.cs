using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtPlanning.ViewModels
{
    public class UserViewModel
    {
        public string ID { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }

        public bool? Active { get; set; }
        public int Language { get; set; }
        public string BirthDate { get; set; }
        public string Color { get; set; }
        public string Initials { get; set; }
        public string LastConnectionDate { get; set; }
        public string AddedLabel { get; set; }
        public string ModificationLabel { get; set; }
    }
}