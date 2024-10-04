using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
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

    protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
    {
#if DEBUG
        Console.WriteLine( "Loading Cave Data Base..." );
        Console.WriteLine( $"{Config.CaveDbPath}" );

        var dbPath = new string( $"Filename={Config.CaveDbPath}" );
        optionsBuilder.UseSqlite( dbPath );
#else
        optionsBuilder.UseSqlite( $"Filename={Config.CaveDbPath}" );
#endif
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
