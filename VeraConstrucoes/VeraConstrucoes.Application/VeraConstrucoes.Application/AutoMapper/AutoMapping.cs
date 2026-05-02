using AutoMapper;
using NFCVeraConstrucoes;
using NFe.Classes;
using VeraConstrucoes.Communication.DTO;
using VeraConstrucoes.Communication.Request;
using VeraConstrucoes.Communication.Response;
using VeraConstrucoes.Domain.Entities.NFCe;
using VeraConstrucoes.Domain.Entities.Produtos;

namespace VeraConstrucoes.Application.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToEntity();
            EntityToResponse();
        }

        private void RequestToEntity()
        {
            CreateMap<NFCeRequestDto, NFC>();
            CreateMap<RequestUpdateProductJson, Produto>()
                // Ignora a propriedade de navegação para evitar o erro de conversão de string para objeto
                .ForMember(dest => dest.ncm, opt => opt.Ignore());
            // Garante que a string Ncm da requisição vá para a string Ncm da entidade
            //.ForMember(dest => dest.Ncm, opt => opt.MapFrom(src => src.Ncm));
        }
        private void EntityToResponse()
        {

            CreateMap<Produto, ResponseRegisterProduct>();
            CreateMap<Produto, RequestUpdateProductJson>();
            CreateMap<Produto, ProdutoDto>()
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.descricao))
                .ForMember(dest => dest.NCM, opt => opt.MapFrom(src => src.Ncm))
                .ForMember(dest => dest.ValorUnitario, opt => opt.MapFrom(src => src.valorUnitario))
                .ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.estoque))
                .ForMember(dest => dest.CFOP, opt => opt.MapFrom(src => 5102)) // Valor padrão
                .ForMember(dest => dest.Unidade, opt => opt.MapFrom(src => "UN")); // Valor padrão
                
        }

       

    }
}


    
