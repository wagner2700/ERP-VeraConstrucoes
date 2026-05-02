using NFe.Utils.NFe;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NFCVeraConstrucoes.NfeExtensions
{
    public static class NfeExtensions
    {
        public static void SalvarXmlEmDisco(this NFe.Classes.NFe nfce, string caminhoArquivo)
        {
            try
            {
                // Garantir que o diretório existe
                var diretorio = Path.GetDirectoryName(caminhoArquivo);
                if (!string.IsNullOrEmpty(diretorio) && !Directory.Exists(diretorio))
                    Directory.CreateDirectory(diretorio);

                // Obter a string XML
                string xmlString = nfce.ObterXmlString();

                // Formatar o XML para ficar legível
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlString);

                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\n",
                    NewLineHandling = NewLineHandling.Replace
                };

                using (var writer = XmlWriter.Create(caminhoArquivo, settings))
                {
                    xmlDoc.Save(writer);
                }

                Console.WriteLine($"✅ XML salvo em: {caminhoArquivo}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao salvar XML: {ex.Message}");
                throw;
            }
        }

        public static string ObterXmlString(this NFe.Classes.NFe nfce)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(NFe.Classes.NFe));
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "http://www.portalfiscal.inf.br/nfe");

                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\n",
                    NewLineHandling = NewLineHandling.Replace,
                    Encoding = Encoding.UTF8
                };

                using (var stringWriter = new StringWriterWithEncoding(Encoding.UTF8))
                using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    xmlSerializer.Serialize(xmlWriter, nfce, namespaces);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao gerar string XML: {ex.Message}");
                throw;
            }
        }

        // Classe auxiliar para garantir o encoding UTF-8
        private class StringWriterWithEncoding : StringWriter
        {
            public override Encoding Encoding { get; }

            public StringWriterWithEncoding(Encoding encoding)
            {
                Encoding = encoding;
            }
        }
    }
}
