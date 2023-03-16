namespace apoio_decisao_medica.Models
{
    public class Medico
    {
        public int Id { get; set; }
        public int Bi { get; set; }
        public string Nome { get; set; }
        public string Especialidade { get; set; }

        public virtual ICollection<Processo>? Processos { get; set; }

    }
}
