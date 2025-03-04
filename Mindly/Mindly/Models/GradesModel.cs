using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Mindly.Models
{
    [Table("grades")]
    public class Grades : BaseModel
    {
        [PrimaryKey("id")]
        public int id { get; set; }

        [Column("student_id")]
        public int studnt_id { get; set; }

        [Column("course_id")]
        public int course_id { get; set; }

        [Column("grade")]
        public int grade { get; set; }

        [Column("created_at")]
        public DateTime created_at { get; set; }

        [Column("updated_at")]
        public DateTime updated_at { get; set; }
    }
}