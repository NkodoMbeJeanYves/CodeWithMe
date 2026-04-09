using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Core.Models
{
    [PrimaryKey(nameof(Subject.subject_id))]
    public class Subject
    {
        public required string subject_id { get; set; }
        public required string subject_name { get; set; }

        public string? description { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
