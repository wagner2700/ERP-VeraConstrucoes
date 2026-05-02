using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeraConstrucoes.Domain.Entities.NCM
{
    public class NCM
    {
        [MaxLength(8)]
        [Key]
        public string Ncm {  get; set; } = string.Empty;
        [MaxLength(200)]
        public string? descricao {  get; set; } = string.Empty ;
        [Column(TypeName = "int(6)")]
        public int csosn {  get; set; }
        
    }
}
