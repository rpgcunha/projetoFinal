using System;

namespace apoio_decisao_medica.Models
{
    public class CatDoenca
    {

        public int Id { get; set; }
        public string Nome { get; set; }

        public virtual ICollection<Doenca>? Doencas { get; set; }

    }
}
