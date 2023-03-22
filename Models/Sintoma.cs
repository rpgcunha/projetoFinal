using System.ComponentModel.DataAnnotations;

namespace apoio_decisao_medica.Models
{
    public class Sintoma
    {
        public int Id { get; set; }

        [Display(Name = "Nome do Sintoma")]
        public string Nome { get; set; }

        [Display(Name = "Categoria")]
        public int CatSintomaId { get; set;}

        public virtual CatSintoma? CatSintoma { get; set; }

        public virtual ICollection<DoencaSintoma>? DoencaSintoma { get; set; }
        public virtual ICollection<ProcessoSintoma>? ProcessoSintomas { get; set; }


    }
}
