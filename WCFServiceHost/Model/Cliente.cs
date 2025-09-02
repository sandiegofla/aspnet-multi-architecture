namespace WCFServiceHost.Model
{
    public class Cliente
    {
        public int Id { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }
        public string RG { get; set; }
        public string DataExpedicao { get; set; }
        public string OrgaoExpedicao { get; set; }
        public string UFExpedicao { get; set; }
        public string DataNascimento { get; set; }
        public string Sexo { get; set; }
        public string EstadoCivil { get; set; }
    }
}
