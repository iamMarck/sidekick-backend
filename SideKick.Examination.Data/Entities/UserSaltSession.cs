using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SideKick.Examination.Data.Entities
{
    public class UserSaltSession
    {
        [Key]
        public int Id { get; set; }
        public long UserId { get; set; }

        [MaxLength(64)]
        public string Salt { get; set; }

        public DateTime DateCreated { get; set; }

        public bool Expired { get; set; }

        public User User { get; set; }
    }
}
