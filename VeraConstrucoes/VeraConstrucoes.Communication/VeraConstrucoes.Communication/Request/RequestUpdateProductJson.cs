using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeraConstrucoes.Communication.Request
{
    public class RequestUpdateProductJson
    {
        public int id { get; set; } 
        
        public string descricao { get; set; } = string.Empty;
        
        public decimal valorUnitario { get; set; }
        public int estoque { get; set; }

        public string? Ncm { get; set; } = string.Empty;
    }
}
