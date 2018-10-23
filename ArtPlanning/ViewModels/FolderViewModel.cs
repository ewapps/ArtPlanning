using System.ComponentModel.DataAnnotations;

namespace ArtPlanning.ViewModels
{
    public class FolderViewModel
    {
        [Key]
        public int ID { get; set; }
        
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        [MaxLength(250)]
        public string CustomerName { get; set; }
    }
}