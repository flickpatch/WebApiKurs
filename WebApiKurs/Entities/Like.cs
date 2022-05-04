using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiKurs.Entities
{
    [Table("Likes")]
    public class Like
    {
        [Key]
        public int id { get; set; }
        [Required]
        public bool IsLiked { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("UserID")]
        public virtual User user { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product product { get; set; }

    }
}
