using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAniversario.Models;

namespace WebAniversario.Data
{
    public class WebAniversarioContext : DbContext
    {
        public WebAniversarioContext (DbContextOptions<WebAniversarioContext> options)
            : base(options)
        {
        }

        public DbSet<WebAniversario.Models.Pessoa> Pessoa { get; set; }
    }
}
