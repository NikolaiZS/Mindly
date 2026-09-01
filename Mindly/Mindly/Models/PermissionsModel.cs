using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Mindly.Models
{
    [Table("permissions")]
    public class Permissions : BaseModel
    {
        [PrimaryKey("id")]
        public int id { get; set; }

        [Column("role_id")]
        public int role_id { get; set; }

        [Column("permission_name")]
        public string permission_name { get; set; }

        [Column("created_at")]
        public DateTime created_at { get; set; }

        [Column("updated_at")]
        public DateTime updated_at { get; set; }
    }
}