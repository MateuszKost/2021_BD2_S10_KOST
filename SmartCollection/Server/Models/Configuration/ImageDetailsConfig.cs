using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Server.Models.Configuration
{
    public class ImageDetailsConfig : IEntityTypeConfiguration<ImageDetails>
    {
        public void Configure(EntityTypeBuilder<ImageDetails> builder)
        {
            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.Date).HasColumnType("TIMESTAMP(0)").IsRequired();
        }

    }
}
