namespace apoio_decisao_medica.Models
{
    public class Utente
    {
        public int Id { get; set; }
        public int NumeroUtente { get; set; }
        public string Nome { get; set; }
        public string DataNascimento { get; set; }
        public string? Genero { get; set; }
        public string? Cidade { get; set; }

        public virtual ICollection<Processo>? Processos { get; set; }
    }
}
