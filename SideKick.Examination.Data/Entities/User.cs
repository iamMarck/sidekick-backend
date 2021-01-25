using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SideKick.Examination.Data.Entities
{
    public class User: BaseEntity
    {
        public User()
        {
            UserSaltSessions = new HashSet<UserSaltSession>();
        }

        [Key]
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }


        public virtual ICollection<UserSaltSession> UserSaltSessions { get; set; }
    }
}
