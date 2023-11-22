using System.ComponentModel.DataAnnotations;

namespace ChatGPTRunner.Models
{
    public class GenRequest
    {
        public int Id { get; set; }

        [Display(Name = "Requesting User")]
        public string OwningUserId { get; set; }

        [MinLength(5)]
        [MaxLength(1024)]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Requested Date")]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Generated Date")]
        public DateTime? GeneratedDate { get; set; }

        [MinLength(5)]
        [MaxLength(1024)]
        public string Actor { get; set; }

        public string Status { get; set; }

        public string? GeneratedContent { get; set; }
        public string? GeneratedTitle { get; set; }
    }
}
