using System.ComponentModel.DataAnnotations;

namespace apoio_decisao_medica.Models
{
    public class Hospital
    {
        public int Id { get; set; }

        [Display(Name = "Nome da Instituição Médica")]
        public string Nome { get; set; }
        public string Cidade { get; set; }

        public virtual ICollection<Processo>? Processos { get; set; }

    }
}
