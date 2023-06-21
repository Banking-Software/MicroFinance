using MicroFinance.Models.AccountSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroFinance.Configuration.LedgerSetup
{
    public class BankSetupConfiguration : IEntityTypeConfiguration<BankSetup>
    {
        public void Configure(EntityTypeBuilder<BankSetup> builder)
        {
            builder.HasIndex(bs => new { bs.LedgerId, bs.Name }).IsUnique();
            builder.Property(bs=>bs.Name).HasConversion(name=>name.ToUpper(), name=>name);

            builder.HasOne(bs=>bs.Ledger)
            .WithOne(l=>l.BankSetup)
            .HasForeignKey<BankSetup>(bs=>bs.LedgerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        }
    }
}