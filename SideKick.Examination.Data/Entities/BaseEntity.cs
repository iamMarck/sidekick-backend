using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SideKick.Examination.Data.Entities
{
    public abstract class BaseEntity
    {
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }

        public void MarkDeleted(DateTime? dateDeleted = null)
        {
            this.DateDeleted = dateDeleted ?? DateTime.UtcNow;
        }
    }
}
