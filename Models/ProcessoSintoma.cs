namespace apoio_decisao_medica.Models
{
    public class ProcessoSintoma
    {
        public int Id { get; set; }
        public int ProcessoId { get; set; }
        public int SintomaId { get; set; }

        public virtual Processo? Processo { get; set; }
        public virtual Sintoma? Sintoma { get; set; }
    }
}
