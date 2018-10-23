using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtPlanning.ViewModels
{
    public class AgendaTaskViewModel
    {

        #pragma warning disable IDE1006 // Styles d'affectation de noms

        public int id { get; set; }
        public string title { get; set; }
        public string comment { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public bool day { get; set; }
        public bool @fixed { get; set; }
        public string eventColor { get; set; }
        public string className { get; set; }
        public string resourceId { get; set; }
        public List<string> staff { get; set; }
        public List<int> vehicles { get; set; }
        public List<int> materials { get; set; }
        public List<int> services { get; set; }
        public string missionType { get; set; }
        public string folderName { get; set; }
        public string folderDescription { get; set; }
        public string type { get; set; }
        public string street1 { get; set; }
        public string zip { get; set; }
        public string city { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public string clientName { get; set; }

        public string addedLabel { get; set; }
        public string modificationLabel { get; set; }

        #pragma warning restore IDE1006 // Styles d'affectation de noms
    }
}