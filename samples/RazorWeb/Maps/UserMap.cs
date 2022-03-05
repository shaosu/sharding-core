using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorWeb.Models;

namespace RazorWeb.Maps
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(o => o.Index);
            builder.Property(o => o.Index).IsRequired();
            builder.Property(o => o.Name).HasMaxLength(128);
            builder.ToTable(nameof(User));
        }
    }
    public class UserBMap : IEntityTypeConfiguration<UserB>
    {
        public void Configure(EntityTypeBuilder<UserB> builder)
        {
            builder.HasKey(o => o.Index);
            builder.Property(o => o.Index).IsRequired();
            builder.Property(o => o.Name).HasMaxLength(128);
            builder.ToTable(nameof(UserB));
        }
    }
}
