namespace apoio_decisao_medica.Models
{
    public class CatSintoma
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        
        public virtual ICollection<Sintoma>? Sintomas { get; set; }

    }
}
