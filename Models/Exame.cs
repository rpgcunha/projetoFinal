namespace apoio_decisao_medica.Models
{
    public class Exame
    {

        public int Id { get; set; }
        public string Nome { get; set; }
        public int CatExameId { get; set;}

        public virtual CatExame? CatExame { get; set; }
        public virtual ICollection<DoencaExame>? DoencaExames { get; set; }
        public virtual ICollection<ProcessoExame>? ProcessoExames { get; set; }
    }
}
