import { Endereco } from "./IEndereco.interface";

export interface ConfiguracaoSistema {
  emitente: {
    razaoSocial: string;
    cnpj: string;
    endereco: Endereco;
    telefone: string;
  };
  sefaz: {
    ambiente: 'producao' | 'homologacao';
    serieNFCe: number;
    ultimoNumero: number;
  };
}