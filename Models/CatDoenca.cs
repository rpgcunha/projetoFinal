using System;
using System.ComponentModel.DataAnnotations;

namespace apoio_decisao_medica.Models
{
    public class CatDoenca
    {

        public int Id { get; set; }

        [Display(Name = "Nome da Categoria")]
        public string Nome { get; set; }

        public virtual ICollection<Doenca>? Doencas { get; set; }

    }
}
