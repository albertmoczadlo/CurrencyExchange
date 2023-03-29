using JediApp.Database.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using System;
using System.Runtime.ConstrainedExecution;


namespace JediApp.Web.Areas.Identity.Data;

public class JediAppDbContext : IdentityDbContext<User>
{

    public DbSet<User> Users { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<WalletPosition> WalletPositions { get; set; }
    public DbSet<Currency> Currencys { get; set; }
    public DbSet<TransactionHistory> TransactionHistory { get; set; }
    public DbSet<ExchangeOffice> ExchangeOffices { get; set; }
    public DbSet<ExchangeOfficeBoard> ExchangeOfficeBoards { get; set; }
    public DbSet<MoneyOnStock> MoneyOnStocks { get; set; }
    public DbSet<CurrencyDictionary> CurrencyDictionaries { get; set; }
    public DbSet<UserAlarm> UserAlarms { get; set; }

    public JediAppDbContext()
    {

    }

    public JediAppDbContext(DbContextOptions<JediAppDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());

        {
            builder.Entity<TransactionHistory>()
                .HasOne(u => u.User)
                .WithMany(x => x.TransactionHistory)
                .HasForeignKey(x => x.UserId);
        }

        {
            builder.Entity<ExchangeOffice>()
            .HasOne<ExchangeOfficeBoard>(p => p.ExchangeOfficeBoard)
            .WithOne(pp => pp.ExchangeOffice)
            .HasForeignKey<ExchangeOfficeBoard>(pp => pp.ExchangeOfficeId)
            .OnDelete(DeleteBehavior.Restrict);
        }

        {
            builder.Entity<Wallet>()
                .HasOne<User>(s => s.User)
                .WithOne(ta => ta.Wallet)
                .HasForeignKey<Wallet>(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        {
            builder.Entity<UserAlarm>()
                .HasOne<User>(s => s.User)
                .WithMany(ta => ta.UserAlarms)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        builder.Entity<Currency>(entity =>
        {
            entity.Property(e => e.BuyAt).HasPrecision(18, 4);
            entity.Property(e => e.SellAt).HasPrecision(18, 4);
        });

        builder.Entity<CurrencyDictionary>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
        });
    }
}

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u=>u.FirstName).HasMaxLength(50);
        builder.Property(u=>u.LastName).HasMaxLength(50);
    }
}