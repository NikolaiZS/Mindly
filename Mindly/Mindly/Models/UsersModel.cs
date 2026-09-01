using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Mindly.Models
{
    [Table("users")]
    public class Users : BaseModel
    {
        [PrimaryKey("id")]
        public int id { get; set; }

        [Column("username")]
        public string username { get; set; }

        [Column("password")]
        public string password { get; set; }

        [Column("first_name")]
        public string first_name { get; set; }

        [Column("last_name")]
        public string last_name { get; set; }

        [Column("role_id")]
        public int role_id { get; set; }

        [Column("manager_id")]
        public int manager_id { get; set; }

        [Column("created_at")]
        public DateTime created_at { get; set; }

        [Column("updated_at")]
        public DateTime updated_at { get; set; }
    }
}