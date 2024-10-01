using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QQBotForCSharp.DataBase.Context;

namespace QQBotForCSharp.DataBase;

public partial class CaveContext : DbContext
{
    public DbSet<CaveDbStruct> Cave { get; set; }

    public CaveContext()
    {
    }

    public CaveContext(DbContextOptions<CaveContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder ) =>
        optionsBuilder.UseSqlite( "Filename=Cave.db" );

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
