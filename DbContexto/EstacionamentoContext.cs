using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StopApi.Models;

namespace StopApi.DbContexto
{
    public class EstacionamentoContext : DbContext
    {
        public EstacionamentoContext(DbContextOptions<EstacionamentoContext> options) : base(options) { }

        public DbSet<Veiculo> Veiculos { get; set; }
    }
}