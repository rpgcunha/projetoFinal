namespace apoio_decisao_medica.Models
{
    public class CatExame
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public virtual ICollection<Exame>? Exames { get; set; }

    }
}
