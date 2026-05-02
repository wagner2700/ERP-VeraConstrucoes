using DFe.Utils;
using System.Security.Cryptography.X509Certificates;

namespace NFCVeraConstrucoes.Services
{
    public class CertificadoServices : IDisposable
    {

        private X509Certificate2 _certificado;
        

        public X509Certificate2 CarregarCertificado(string caminhoCertificado, string senha)
        {
            try
            {
                if (!File.Exists(caminhoCertificado))
                    throw new FileNotFoundException($"Certificado não encontrado: {caminhoCertificado}");

                _certificado = new X509Certificate2(
                    caminhoCertificado,
                    senha,
                    X509KeyStorageFlags.MachineKeySet |
                    X509KeyStorageFlags.PersistKeySet |
                    X509KeyStorageFlags.Exportable);

                ValidarCertificado(_certificado);

                return _certificado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar certificado: {ex.Message}", ex);
            }

        }


        public void ValidarCertificado(X509Certificate2 certificado)
        {
            if (!certificado.HasPrivateKey)
                throw new Exception("Certificado não possui chave privada");

            if (DateTime.Now < certificado.NotBefore)
                throw new Exception($"Certificado válido apenas a partir de {certificado.NotBefore:dd/MM/yyyy}");

            if (DateTime.Now > certificado.NotAfter)
                throw new Exception($"Certificado expirou em {certificado.NotAfter:dd/MM/yyyy}");

            // Aviso se faltar menos de 30 dias
            if (certificado.NotAfter < DateTime.Now.AddDays(30))
                Console.WriteLine($"⚠️ ATENÇÃO: Certificado expira em {(certificado.NotAfter - DateTime.Now).Days} dias!");


        }

        public string ExtrairCNPJCertificado()
        {
            if(_certificado == null)
            {
                throw new Exception("Certificado não carregado");
            }

            // O CNPJ está no Subject do certificado
            var subject = _certificado.Subject;

            var cnpj = System.Text.RegularExpressions.Regex.Match(subject, @"\d{14}");

            if (cnpj.Success)
            {
                return cnpj.Value;
            }

            throw new Exception("CNPJ não encontrado no certificado");
        }

        public ConfiguracaoCertificado CarregarConfiguracaoCertificado(string caminhoCertificado, string senha)
        {
            try
            {
                // Carregar certificado
                var cert = CarregarCertificado(caminhoCertificado, senha);

                // Exportar para bytes
                byte[] certificadoBytes = cert.Export(
                    X509ContentType.Pfx,
                    senha
                );

                return new ConfiguracaoCertificado
                {
                    TipoCertificado = TipoCertificado.A1ByteArray,
                    ArrayBytesArquivo = certificadoBytes,
                    Senha = senha
                };
            }
            catch (Exception ex)
            {
                // Fallback: tentar método direto do arquivo
                return new ConfiguracaoCertificado
                {
                    TipoCertificado = TipoCertificado.A1Arquivo,
                    Arquivo = caminhoCertificado,
                    Senha = senha
                };
            }
        }

        public void Dispose()
        {
            _certificado?.Dispose();
        }
    }
}
