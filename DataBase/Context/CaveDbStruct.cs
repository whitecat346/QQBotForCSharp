﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace QQBotForCSharp.DataBase.Context
{
    public class CaveDbStruct
    {
        [Key]
        [Required]
        [Column( TypeName = "INTEGER" )]
        public required int? ID { get; set; }

        [Required]
        [Column( TypeName = "TEXT" )]
        public required string Sender { get; set; }

        [Required]
        [Column( TypeName = "TEXT" )]
        public required string Context { get; set; }
    }
}
