
using Newtonsoft.Json;


namespace NFCVeraConstrucoes.Helpers
{
    public static class ConfigHelper
    {
        private static readonly string ConfigFilePath = "config_nfce.json";
        public static ConfiguracaoNF CarregarConfiguracao()
        {

            try
            {
                //if (!File.Exists(ConfigFilePath))
                //{
                //    // Criar configuração padrão
                //    var configPadrao = CriarConfiguracaoPadrao();
                //    SalvarConfiguracao(configPadrao);
                //    return configPadrao;
                //}

                var json = File.ReadAllText(ConfigFilePath);
                return JsonConvert.DeserializeObject<ConfiguracaoNF>(json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao carregar configuração: {ex.Message}", ex);
            }
        }


        

        public static void SalvarConfiguracao(VeraConstrucoes.Domain.Entities.Configuracoes.ConfiguracaoNFCe configuracao)
        {
            try
            {
                var json = JsonConvert.SerializeObject(configuracao, Formatting.Indented);
                File.WriteAllText(ConfigFilePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar configuração: {ex.Message}", ex);
            }
        }

        

        private static VeraConstrucoes.Domain.Entities.Configuracoes.ConfiguracaoNFCe CriarConfiguracaoPadrao()
        {
            return new VeraConstrucoes.Domain.Entities.Configuracoes.ConfiguracaoNFCe();

        }

        // Método para gerar cNF válido
        public static string GerarCNF()
        {
            Random random = new Random();

            // Gera número entre 1 e 99.999.999
            int numero = random.Next(1, 100000000);

            // Formata com 8 dígitos com zeros à esquerda
            return numero.ToString("D8");
        }



    }
}
