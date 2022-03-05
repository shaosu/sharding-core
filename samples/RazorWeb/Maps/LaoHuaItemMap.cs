using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RazorWeb.Models;

namespace RazorWeb.Maps
{

    public class LaoHuaItemMap : IEntityTypeConfiguration<LaoHuaItem>
    {
        public void Configure(EntityTypeBuilder<LaoHuaItem> builder)
        {
            builder.HasKey(o => o.Index);
            builder.Property(o => o.Index).IsRequired();
            builder.Property(o => o.GUID).HasMaxLength(32);
            builder.Property(o => o.SN).HasMaxLength(32);
            builder.ToTable(nameof(LaoHuaItem));
        }
    }
}
