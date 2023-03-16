namespace apoio_decisao_medica.Models
{
    public class DoencaExame
    {
        public int Id { get; set; }
        public int DoencaId { get; set; }
        public int ExameId { get; set; }
        public int Relevancia { get; set; }

        public virtual Doenca? Doenca { get; set; }
        public virtual Exame? Exame { get; set; }



    }
}
