namespace apoio_decisao_medica.Models
{
    public class Hospital
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cidade { get; set; }

        public virtual ICollection<Processo>? Processos { get; set; }

    }
}
