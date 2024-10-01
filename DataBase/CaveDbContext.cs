using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QQBotForCSharp.DataBase.Context;

namespace QQBotForCSharp.DataBase
{
    public class CaveDbContext : DbContext
    {
        public DbSet<CaveDbStruct> Cave { get; set; }

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            base.OnConfiguring( optionsBuilder );
            optionsBuilder.UseSqlite( "Data Source=Cave.db" );
        }
    }
}
