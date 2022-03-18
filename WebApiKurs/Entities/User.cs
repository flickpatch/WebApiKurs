using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApiKurs.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(48)]
        public string Name { get; set; }
        [Required, MaxLength(48)]
        public string SecName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [JsonIgnore]
        public List<Product> Products { get; set; }
        [Required, MaxLength(64), EmailAddress]
        public string Email { get; set; }
        [Required, MaxLength(64)]
        public string Login { get; set; }
        [Required, MaxLength(64)]
        public string Pass { get; set; }
        public byte[] Photo { get; set; }
        public User()
        {
            Products = new List<Product>();
        }
    }
}
