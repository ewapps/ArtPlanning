using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArtPlanning.Models
{
    public class TaskViewModel
    {        
        public int ID { get; set; }

        public int MissionID { get; set; }
        public int FolderID { get; set; }
        public string Title { get; set; }
        public DateTime? ScheduledStartDate { get; set; }
        public DateTime? ScheduledEndDate { get; set; }
        public bool? Fixed { get; set; }
        public string Comment { get; set; }
        public string StreetLine1 { get; set; }
        public string Number { get; set; }
        public string Box { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
    }
}