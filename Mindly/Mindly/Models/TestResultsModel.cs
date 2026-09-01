using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Mindly.Models
{
    [Table("testresults")]
    public class TestResults : BaseModel
    {
        [PrimaryKey("id")]
        public int id { get; set; }

        [Column("student_id")]
        public int student_id { get; set; }

        [Column("test_id")]
        public int test_id { get; set; }

        [Column("score")]
        public int score { get; set; }

        [Column("created_at")]
        public DateTime created_at { get; set; }

        [Column("updated_at")]
        public DateTime updated_at { get; set; }
    }
}