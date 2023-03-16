namespace apoio_decisao_medica.Models
{
    public class DoencaSintoma
    {
        public int Id { get; set; }
        public int DoencaId { get; set; }
        public int SintomaId { get; set; }
        public int Relevancia { get; set; }

        public virtual Doenca? Doenca { get; set; }
        public virtual Sintoma? Sintoma { get; set; }
    }
}
