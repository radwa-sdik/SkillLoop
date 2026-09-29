using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkillLoop.Domain.Entities;

namespace SkillLoop.Infrasturcture.Data.Configurations
{
    public class CreditPackageConfiguration : IEntityTypeConfiguration<CreditPackage>
    {
        public void Configure(EntityTypeBuilder<CreditPackage> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PriceEgp)
                .HasPrecision(18, 2);
        }
    }
}
