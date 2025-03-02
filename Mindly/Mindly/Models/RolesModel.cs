using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;


namespace Mindly.Models
{
    [Table("roles")]
    public class Roles : BaseModel
    {
        [PrimaryKey("id")]
        public int id { get; set; }
        [Column("name")]
        public string name { get; set; }
    }
}
