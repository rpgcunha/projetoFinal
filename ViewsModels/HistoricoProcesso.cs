namespace apoio_decisao_medica.ViewsModels
{
    public class HistoricoProcesso
    {
        public int? Id { get; set; }
        public int NumProcesso { get; set; }
        public int? UtenteId { get; set; }
        public string? NomeUtente { get; set; }
        public int? NumeroUtente { get; set; }
        public string DataAbertura { get; set; }
        public string? DataFecho { get; set; }
        public string? Doenca { get; set; }
        public string? Medico { get; set; }
        public string Hospital { get; set; }
    }
}
