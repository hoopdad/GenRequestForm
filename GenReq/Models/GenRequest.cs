using System.ComponentModel.DataAnnotations;

namespace GenReq.Models
{
    public class GenRequest
    {
        public int id { get; set; }
        public string OwningUserId { get; set; }
        public string? Title { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime GeneratedDate { get; set; }
        public string? Actor { get; set; }
        public string Status { get; set; }
    }
}
