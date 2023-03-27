using System.ComponentModel.DataAnnotations;

namespace apoio_decisao_medica.Models
{
	public class Utilizador
	{
		public int Id { get; set; }

        [Display(Name = "Username")]
        public string User { get; set; }

        [Display(Name = "Password")]
        public string Pass { get; set; }

        [Display(Name = "Nome do Médico")]
        public int MedicoId { get; set; }

        [Display(Name = "É Admin?")]
        public bool IsAdmin { get; set; }

		public virtual Medico? Medico { get; set; }
	}
}
