using System.ComponentModel.DataAnnotations;

namespace apoio_decisao_medica.Models
{
    public class Doenca
    {
        public int Id { get; set; }

        [Display(Name = "Nome da Doença")]
        public string Nome { get; set; }

        [Display(Name = "Categoria da Doença")]
        public int CatDoencaId { get; set; }

        public virtual CatDoenca? CatDoenca { get; set; }

        public virtual ICollection<DoencaExame>? DoencaExames { get; set; }
        public virtual ICollection<DoencaSintoma>? DoencaSintoma { get; set; }
        public virtual ICollection<Processo>? Processos { get; set; }


    }


}
