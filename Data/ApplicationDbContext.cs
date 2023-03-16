using Microsoft.EntityFrameworkCore;
using apoio_decisao_medica.Models;
using System;


namespace apoio_decisao_medica.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Doenca> Tdoencas { get; set; }
        public DbSet<CatDoenca> TcatDoencas { get; set; }

        public DbSet<Exame> Texames { get; set; }
        public DbSet<CatExame> TcatExames { get; set; }
        public DbSet<DoencaExame> TdoencaExames { get; set; }

        public DbSet<Sintoma> Tsintomas { get; set; }
        public DbSet<CatSintoma> TcatSintomas { get; set; }

        public DbSet<DoencaSintoma> TdoencaSintomas { get; set; }
        public DbSet<Utente> Tutentes { get; set; }
        public DbSet<Medico> Tmedicos { get; set; }
        public DbSet<Hospital> Thospitais { get; set; }
        public DbSet<Processo> Tprocessos { get; set; }

        public DbSet<ProcessoExame> TprocessoExames { get; set; }
        public DbSet<ProcessoSintoma> TprocessoSintomas { get; set; }





    }
}
