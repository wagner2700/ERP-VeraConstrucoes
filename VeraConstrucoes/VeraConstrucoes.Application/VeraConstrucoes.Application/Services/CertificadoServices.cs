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
                var senhaResolvida = ResolverSenhaCertificado(senha);
                var certificadoBytes = TentarLerCertificadoBase64();

                if (certificadoBytes is not null)
                {
                    _certificado = CriarCertificado(certificadoBytes, senhaResolvida);
                    ValidarCertificado(_certificado);
                    return _certificado;
                }

                var caminhoResolvido = ResolverCaminhoCertificado(caminhoCertificado);

                if (string.IsNullOrWhiteSpace(caminhoResolvido))
                    throw new FileNotFoundException("Certificado não configurado.");

                if (!Path.IsPathRooted(caminhoResolvido))
                    caminhoResolvido = Path.GetFullPath(caminhoResolvido, Directory.GetCurrentDirectory());

                if (!File.Exists(caminhoResolvido))
                    throw new FileNotFoundException($"Certificado não encontrado: {caminhoResolvido}");

                _certificado = CriarCertificado(caminhoResolvido, senhaResolvida);

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
            var senhaResolvida = ResolverSenhaCertificado(senha);
            var certificadoBytes = TentarLerCertificadoBase64();
            var caminhoResolvido = ResolverCaminhoCertificado(caminhoCertificado);

            if (certificadoBytes is not null)
            {
                return new ConfiguracaoCertificado
                {
                    TipoCertificado = TipoCertificado.A1ByteArray,
                    ArrayBytesArquivo = certificadoBytes,
                    Senha = senhaResolvida
                };
            }

            try
            {
                // Carregar certificado
                var cert = CarregarCertificado(caminhoResolvido, senhaResolvida);

                // Exportar para bytes
                byte[] certificadoExportadoBytes = cert.Export(
                    X509ContentType.Pfx,
                    senhaResolvida
                );

                return new ConfiguracaoCertificado
                {
                    TipoCertificado = TipoCertificado.A1ByteArray,
                    ArrayBytesArquivo = certificadoExportadoBytes,
                    Senha = senhaResolvida
                };
            }
            catch (Exception)
            {
                // Fallback: tentar método direto do arquivo
                return new ConfiguracaoCertificado
                {
                    TipoCertificado = TipoCertificado.A1Arquivo,
                    Arquivo = caminhoResolvido,
                    Senha = senhaResolvida
                };
            }
        }

        private static X509Certificate2 CriarCertificado(string caminhoCertificado, string senha)
        {
            return new X509Certificate2(
                caminhoCertificado,
                senha,
                X509KeyStorageFlags.MachineKeySet |
                X509KeyStorageFlags.PersistKeySet |
                X509KeyStorageFlags.Exportable);
        }

        private static X509Certificate2 CriarCertificado(byte[] certificadoBytes, string senha)
        {
            return new X509Certificate2(
                certificadoBytes,
                senha,
                X509KeyStorageFlags.MachineKeySet |
                X509KeyStorageFlags.PersistKeySet |
                X509KeyStorageFlags.Exportable);
        }

        private static string ResolverCaminhoCertificado(string caminhoCertificado)
        {
            return PrimeiroValorPreenchido(
                Environment.GetEnvironmentVariable("NFC_CERT_PATH"),
                Environment.GetEnvironmentVariable("Certificado__Arquivo"),
                caminhoCertificado) ?? string.Empty;
        }

        private static string ResolverSenhaCertificado(string senha)
        {
            return PrimeiroValorPreenchido(
                Environment.GetEnvironmentVariable("NFC_CERT_PASSWORD"),
                Environment.GetEnvironmentVariable("Certificado__Senha"),
                senha) ?? string.Empty;
        }

        private static byte[]? TentarLerCertificadoBase64()
        {
            var certificadoBase64 = PrimeiroValorPreenchido(
                Environment.GetEnvironmentVariable("NFC_CERT_BASE64"),
                Environment.GetEnvironmentVariable("Certificado__Base64"));

            if (string.IsNullOrWhiteSpace(certificadoBase64))
                return null;

            try
            {
                return Convert.FromBase64String(certificadoBase64.Trim());
            }
            catch (FormatException ex)
            {
                throw new InvalidOperationException("A variavel de ambiente do certificado base64 esta invalida.", ex);
            }
        }

        private static string? PrimeiroValorPreenchido(params string?[] valores)
        {
            foreach (var valor in valores)
            {
                if (!string.IsNullOrWhiteSpace(valor))
                    return valor;
            }

            return null;
        }

        public void Dispose()
        {
            _certificado?.Dispose();
        }
    }
}

