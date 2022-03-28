using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiKurs.Entities
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(48)]
        public string Name { get; set; }
        [Required, MaxLength(256)]
        public string Description { get; set; }
        [Required]
        public DateTime DateCreate { get; set; }
        [Required]
        public bool IsActivity { get; set; }
        [Required]
        public int Price { get; set; }        
        public byte[] Photo { get; set; }       
        [Required]
        public int UserId { get; set; }
        [Required, MaxLength(48)]
        public string Type { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
       
    }
}
