using System.ComponentModel.DataAnnotations;

namespace apoio_decisao_medica.Models
{
    public class Medico
    {
        public int Id { get; set; }

        [Display(Name = "Numero do BI")]
        [RegularExpression("[0-9]{8}", ErrorMessage = "Numero do BI incorreto")]
        public int Bi { get; set; }
        public string Nome { get; set; }

        [Display(Name = "ID do Utilizador")]
        public int UtilizadorId { get; set; }
        public string Especialidade { get; set; }

		public virtual Utilizador? Utilizador { get; set; }
		public virtual ICollection<Processo>? Processos { get; set; }

    }
}
