namespace apoio_decisao_medica.Models
{
    public class ProcessoExame
    {
        public int Id { get; set; }
        public int ProcessoId { get; set; }
        public int ExameId { get; set;}

        public virtual Processo? Processo { get; set; }
        public virtual Exame? Exame { get; set; }

    }
}
