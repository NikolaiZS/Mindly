using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;


namespace Mindly.Models
{
    [Table("lessons")]
    public class Lessons : BaseModel
    {
        [PrimaryKey("id")]
        public int id { get; set; }
        [Column("course_id")]
        public int course_id { get; set; }
        [Column("title")]
        public string title { get; set; }
        [Column("description")]
        public string description { get; set; }
        [Column("teacher_id")]
        public int teacher_id { get; set; }
        [Column("date")]
        public DateTime date { get; set; }
        [Column("created_at")]
        public DateTime created_at { get; set; }
        [Column("updated_at")]
        public DateTime updated_at { get; set; }
    }
}
