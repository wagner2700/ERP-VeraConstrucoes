using Microsoft.EntityFrameworkCore;
using NFe.Classes;
using VeraConstrucoes.Communication.DTO;
using VeraConstrucoes.Domain.Entities.NFCe;
using VeraConstrucoes.Infrastructure.Data;
using VeraConstrucoes.Infrastructure.Repositories.Interfaces;

namespace VeraConstrucoes.Infrastructure.Repositories
{
    public class NFCRepository : INFCRepository
    {
        private readonly VeraConstrucoesContext _context;

        public NFCRepository(VeraConstrucoesContext context) => _context = context;


        public async Task<NFC> AddNFC(NFC Nfc)
        {
            // Carrega a NFC existente com todos os relacionamentos
            var existingNfc = await _context.NFCes
                .Include(n => n.Complemento)
                .Include(n => n.Produtos)
                .Include(n => n.Pagamentos)
                .FirstOrDefaultAsync(n => n.Id == Nfc.Id);

            if (existingNfc != null)
            {
                // --- Atualiza propriedades escalares da NFC ---
                existingNfc.Numero = Nfc.Numero;
                existingNfc.Serie = Nfc.Serie;
                existingNfc.DataEmissao = Nfc.DataEmissao;
                existingNfc.ValorTotal = Nfc.ValorTotal;
                existingNfc.SituacaoNota = Nfc.SituacaoNota;
                existingNfc.ClienteDocumento = Nfc.ClienteDocumento;
                existingNfc.ClienteNome = Nfc.ClienteNome;
                existingNfc.ClienteEmail = Nfc.ClienteEmail;
                existingNfc.ClienteTelefone = Nfc.ClienteTelefone;
                existingNfc.StatusProcessamento = Nfc.StatusProcessamento;
                // --- Atualiza o Complemento (1:1) ---
                if (Nfc.Complemento != null)
                {
                    if (existingNfc.Complemento == null)
                    {
                        // Adiciona novo complemento (a chave estrangeira será preenchida pelo EF)
                        existingNfc.Complemento = Nfc.Complemento;
                    }
                    else
                    {
                        // Atualiza complemento existente
                        existingNfc.Complemento.Observacoes = Nfc.Complemento.Observacoes;
                        existingNfc.Complemento.Desconto = Nfc.Complemento.Desconto;
                        existingNfc.Complemento.Acrescimo = Nfc.Complemento.Acrescimo;
                        existingNfc.Complemento.XmlPath = Nfc.Complemento.XmlPath;
                        existingNfc.Complemento.PdfPath = Nfc.Complemento.PdfPath;
                        existingNfc.Complemento.QrCodeUrl = Nfc.Complemento.QrCodeUrl;
                        existingNfc.Complemento.MensagemErro = Nfc.Complemento.MensagemErro;
                        existingNfc.Complemento.ChaveAcesso = Nfc.Complemento.ChaveAcesso;
                        existingNfc.Complemento.Protocolo = Nfc.Complemento.Protocolo;
                    }
                }
                
            }
            else
            {
                // Nova NFC: apenas adiciona (as FK serão preenchidas automaticamente se configuradas)
                await _context.NFCes.AddAsync(Nfc);
            }

            await _context.SaveChangesAsync();
            return Nfc;
        }



        public async Task UpdateSituacaoNotaFiscal(int IdNota , string situacaoNota)
        {
            var notaFiscal = await _context.NFCes.FirstOrDefaultAsync(n => n.Id == IdNota);
            if (notaFiscal != null)
            {
                notaFiscal.SituacaoNota = situacaoNota;
                await _context.SaveChangesAsync();
            }
        }

        


        public async Task<NFC> AddOrUpdateNFC(NFC nfc)
        {
            
        var existingNfc = await _context.NFCes
            .Include(n => n.Complemento)
            .Include(n => n.Produtos)
            .Include(n => n.Pagamentos)
            .FirstOrDefaultAsync(n => n.Id == nfc.Id);

            if (existingNfc != null)
            {
                // Atualiza campos escalares
                _context.Entry(existingNfc).CurrentValues.SetValues(nfc);

                // Atualiza complemento
                if (nfc.Complemento != null)
                {
                    if (existingNfc.Complemento == null)
                    {
                        existingNfc.Complemento = nfc.Complemento;
                    }
                    else
                    {
                        _context.Entry(existingNfc.Complemento).CurrentValues.SetValues(nfc.Complemento);
                    }
                }

                // Atualiza produtos (simplificado: remove todos e adiciona os novos)
                //_context.ProdutoNFCes.RemoveRange(existingNfc.Produtos);
                //foreach (var prod in nfc.Produtos)
                //{
                //    prod.NFCeId = existingNfc.Id;
                //    existingNfc.Produtos.Add(prod);
                //}

                //// Atualiza pagamentos
                //_context.PagamentoNFCes.RemoveRange(existingNfc.Pagamentos);
                //foreach (var pag in nfc.Pagamentos)
                //{
                //    pag.NFCeId = existingNfc.Id;
                //    existingNfc.Pagamentos.Add(pag);
                //}
            }
            else
            {
                await _context.NFCes.AddAsync(nfc);
            }

            await _context.SaveChangesAsync();
            return existingNfc ?? nfc;
        }

        public async Task<NFC?> GetByChaveAcessoAsync(string chaveAcesso)
        {
            // Busca a NFC através da propriedade Complemento.ChaveAcesso
            return await _context.NFCes
                .Include(n => n.Complemento)
                .Include(n => n.Produtos)
                .Include(n => n.Pagamentos)
                .FirstOrDefaultAsync(n => n.Complemento != null && n.Complemento.ChaveAcesso == chaveAcesso);
        }

        public async Task<NfcDetalhesDTO?> ObterDetalhesCompletoAsync(int id)
        {
            var nfce = await _context.NFCes
                .Include(n => n.Complemento)
                .Include(n => n.Produtos)
                .Include(n => n.Pagamentos)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (nfce == null)
                return null;

            return new NfcDetalhesDTO
            {
                Id = nfce.Id,
                Numero = nfce.Numero,
                Serie = nfce.Serie,
                DataEmissao = nfce.DataEmissao,
                ValorTotal = nfce.ValorTotal,
                StatusProcessamento = nfce.StatusProcessamento,
                ClienteDocumento = nfce.ClienteDocumento,
                ClienteNome = nfce.ClienteNome,
                ClienteEmail = nfce.ClienteEmail,
                ClienteTelefone = nfce.ClienteTelefone,
                SituacaoNota = nfce.SituacaoNota,

                // Dados do complemento (podem ser nulos)
                Observacoes = nfce.Complemento?.Observacoes,
                Desconto = nfce.Complemento?.Desconto ?? 0,
                Acrescimo = nfce.Complemento?.Acrescimo ?? 0,
                XmlPath =   Path.GetFullPath(nfce.Complemento?.XmlPath),
                PdfPath = nfce.Complemento?.PdfPath,
                QrCodeUrl = nfce.Complemento?.QrCodeUrl,
                MensagemErro = nfce.Complemento?.MensagemErro,
                ChaveAcesso = nfce.Complemento?.ChaveAcesso,
                Protocolo = nfce.Complemento?.Protocolo,

                // Produtos
                Produtos = nfce.Produtos.Select(p => new ProdutoNfcDTO
                {
                    Codigo = p.Codigo,
                    Descricao = p.Descricao,
                    Quantidade = p.Quantidade,
                    ValorUnitario = p.ValorUnitario,
                    Ncm = p.Ncm,
                    Cfop = p.Cfop,
                    Unidade = p.Unidade,
                    Cest = p.Cest
                }).ToList(),

                // Pagamentos
                Pagamentos = nfce.Pagamentos.Select(pg => new PagamentoNfcDTO
                {
                    Metodo = pg.Metodo,
                    Valor = pg.Valor,
                    Parcelas = pg.Parcelas,
                    Troco = pg.Troco,
                    Bandeira = pg.Bandeira
                }).ToList()
            };

        }

        public Task<List<NFCeResumoDTO>> ObterNotasFiscais()
        {
            return _context.NFCes.OrderByDescending(n => n.DataEmissao).Select(n => new NFCeResumoDTO
            {
                Id = n.Id,
                Numero = n.Numero,
                Serie = n.Serie,
                DataEmissao = n.DataEmissao,
                ValorTotal = n.ValorTotal,
                StatusProcessamento = n.StatusProcessamento,
                ClienteNome = n.ClienteNome,
                ClienteDocumento = n.ClienteDocumento
            })
        .ToListAsync();
        }
    }
}
