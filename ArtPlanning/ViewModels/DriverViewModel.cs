using System.ComponentModel.DataAnnotations;

namespace ArtPlanning.ViewModels
{
    public class DriverViewModel
    {
        [Key]
        public string ID { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        public string NameWithInitials { get; set; }
    }

    public class DriverResource
    {
        public string id { get; set; }
        public string title { get; set; }
        public string initials { get; set; }
        public string eventColor { get; set; }
        public string firstname { get; set; }
    }
}