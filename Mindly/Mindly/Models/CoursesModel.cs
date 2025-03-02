using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;


namespace Mindly.Models
{
    [Table("courses")]
    public class Courses : BaseModel
    {
        [PrimaryKey("id")]
        public int id { get; set; }
        [Column("name")]
        public string name { get; set; }
        [Column("description")]
        public string description { get; set; }
        [Column("created_at")]
        public DateTime created_at { get; set; }
        [Column("updated_at")]
        public DateTime updated_at { get; set; }

    }
}
