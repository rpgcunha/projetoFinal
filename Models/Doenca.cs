namespace apoio_decisao_medica.Models
{
    public class Doenca
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int CatDoencaId { get; set; }

        public virtual CatDoenca? CatDoenca { get; set; }

        public virtual ICollection<DoencaExame>? DoencaExames { get; set; }
        public virtual ICollection<DoencaSintoma>? DoencaSintoma { get; set; }
        public virtual ICollection<Processo>? Processos { get; set; }


    }


}
