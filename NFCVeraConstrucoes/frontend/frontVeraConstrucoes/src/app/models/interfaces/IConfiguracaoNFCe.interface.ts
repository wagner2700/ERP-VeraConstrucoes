export interface ConfiguracaoNFCe {
  id: number; // sempre 1
  caminhoCertificado: string;
  senhaCertificado: string;
  serieNota: number;
  ultimoNumeroNota: number;
  cnpjEmitente: string;
  razaoSocial: string;
  nomeFantasia: string;
  inscricaoEstadual: string;
  regimeTributario: number; // CRT: 1-SN, 2-SN excedente, 3-Normal
  logradouroEmitente: string;
  numeroEmitente: string;
  bairroEmitente: string;
  complementoEmitente: string;
  codigoMunicipioIBGE: number;
  nomeMunicipio: string;
  uf: string;
  estado: number; // código IBGE da UF (enum Estado)
  cep: string;
  telefone: number; // agora é long no C#, portanto número
  unidadeFederativaCodigo: number;
  ambiente: number; // TipoAmbiente: 1-produção, 2-homologação
  tipoEmissao: number; // TipoEmissao: 1-normal
  csc: string;
  idCSC: string;
  timeoutTransmissao: number;
  tentativasConsultaRecibo: number;
  intervaloConsultaRecibo: number;
  pastaXmlEnviados: string;
  pastaXmlAutorizados: string;
  pastaXmlCancelados: string;
  pastaDanfes: string;
  versao: number; // VersaoServico (ex: 400 para 4.00)
}
