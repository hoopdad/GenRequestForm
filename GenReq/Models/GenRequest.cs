using System.ComponentModel.DataAnnotations;

namespace GenReq.Models
{
    public enum ContentTemplates
    {
        [Display(Name = "Free Form")]
        Freeform,
        [Display(Name = "Technical Overview")]
        TechnicalOverview = 1,
        [Display(Name = "Learning a New Skill")]
        LearnNewSkill=2,
        [Display(Name = "How To")]
        Howto = 3
    }


    public class GenRequest
    {
        public int Id { get; set; }

        [Display(Name = "Requesting User")]
        public string OwningUserId { get; set; }

        [MinLength(5)]
        [MaxLength(1024)] 
        public string Title { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Requested Date")]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Generated Date")]
        public DateTime? GeneratedDate { get; set; }

        [MinLength(5)]
        [MaxLength(1024)]
        public string Actor { get; set; }
        
        public string Status { get; set; }

        [Display(Name = "Generated Content")]
        public string? GeneratedContent { get; set; }

        [Display(Name = "Generated Title")]
        public string? GeneratedTitle { get; set; }

        [Display(Name = "Content Template")]
        public ContentTemplates ContentTemplate { get; set; } = ContentTemplates.Freeform;
    }
}
