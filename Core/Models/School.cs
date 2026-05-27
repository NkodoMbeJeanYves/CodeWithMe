using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeWithMe.Core.Models
{
    [Table("schools")]
    public class School : HasTimestamps, ITenantScoped
    {
        [Column("school_id")]
        [Key()]
        public required string SchoolId { get; set; }

        [Column("tenant_id")]
        [MaxLength(36)]
        public string TenantId { get; set; } = Tenant.DefaultTenantId;

        [Column("name")]
        public required string Name { get; set; }

        [Column("school_type")]
        public required string SchoolType { get; set; }

        [Column("description")]
        public required string Description { get; set; }

        [Column("class_start_time")]
        public required TimeSpan ClassStartTime { get; set; }

        [Column("class_end_time")]
        public required TimeSpan ClassEndTime { get; set; }

        [Column("class_duration")]
        public int ClassDurationInMinutes { get; set; }

        [Column("first_break_duration")]
        public int FirstBreakDurationInMinutes { get; set; }

        [Column("first_break_start_time")]
        public required TimeSpan FirstBreakStartTime { get; set; }

        [Column("second_break_duration")]
        public int? SecondBreakDurationInMinutes { get; set; }

        [Column("second_break_start_time")]
        public TimeSpan? SecondBreakStartTime { get; set; }

        [Column("third_break_duration")]
        public int? ThirdBreakDurationInMinutes { get; set; }

        [Column("third_break_start_time")]
        public TimeSpan? ThirdBreakStartTime { get; set; }

        // Relationship
        public virtual ICollection<Period> Periods { get; set; } = new List<Period>();
        public virtual ICollection<ProgramModel> Programs { get; set; } = new List<ProgramModel>();
    }
}
