namespace apoio_decisao_medica.Models
{
    public class Processo
    {
        public int Id { get; set; }
        public int NumeroProcesso { get; set; }
        public int UtenteId { get; set; }
        public int MedicoId { get; set; }
        public int HospitalId { get; set; }
        public string DataHoraAbertura { get; set; }
        public string? DataHoraFecho { get; set; }
        public int? DoencaId { get; set; }

        public virtual Utente? Utente { get; set; }
        public virtual Medico? Medico { get; set; }
        public virtual Hospital? Hospital { get; set; }
        public virtual Doenca? Doenca { get; set; }

    }
}
