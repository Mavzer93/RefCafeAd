using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefCafeAData
{
    public class Comment : EntityBase
    {
        public Guid ProductId { get; set; }
        public Guid ApplicationUserId { get; set; }
        public string Text { get; set; }
        public int Rate { get; set; }
        public virtual Product? Product { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }

    }
    public class CommentEntityTypeConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder) 
        {
           
        }
    }
}
