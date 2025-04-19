using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.Configurations;

public class RegisterConfiguration: IEntityTypeConfiguration<Register>
{
    public void Configure(EntityTypeBuilder<Register> builder)
    {
        builder.HasKey(register => register.Id);
    }
}
