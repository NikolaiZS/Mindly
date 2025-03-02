using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Mindly.Models
{
    [Table("assignments")]
    public class  Assignments : BaseModel
    {
        [PrimaryKey("id")]
        public int id { get; set; }
        [Column("student_id")]
        public int student_id { get; set; }
        [Column("teacher_id")]
        public int teacher_id { get; set; }
        [Column("course_id")]
        public int course_id { get; set; }
        [Column("created_at")]
        public DateTime created_at { get; set; }
        [Column("updated_at")]
        public DateTime updated_at { get; set; }
    }
}
