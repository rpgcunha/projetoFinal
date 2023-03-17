namespace apoio_decisao_medica.ViewsModels
{
    public class HistoricoUtente
    {
        public int Id { get; set; }
        public int NumeroProcesso { get; set; }
        public string DataAbertura { get; set; }
        public string? DataFecho { get; set; }
        public string? Doenca { get; set; }
        public string Medico { get; set; }
        public string Hospital { get; set; }
    }
}
