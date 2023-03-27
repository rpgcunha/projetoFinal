using System.ComponentModel.DataAnnotations;

namespace apoio_decisao_medica.Models
{
    public class Utente
    {
        public int Id { get; set; }

        [Display(Name = "Numero de Utente")]
        [RegularExpression("[0-9]{9}", ErrorMessage = "Numero de Utente incorreto")]
        public int NumeroUtente { get; set; }

        [Display(Name = "Nome Completo")]
        public string Nome { get; set; }

        [Display(Name = "Data de Nascimento 'DD/MM/AAAA'")]
        [RegularExpression("[0-9]{2}/[0-9]{2}/[0-9]{4}", ErrorMessage = "Data incorreta")]
        public string DataNascimento { get; set; }

        [Display(Name = "Género")]
        public string? Genero { get; set; }
        public string? Cidade { get; set; }

        public virtual ICollection<Processo>? Processos { get; set; }
    }
}
