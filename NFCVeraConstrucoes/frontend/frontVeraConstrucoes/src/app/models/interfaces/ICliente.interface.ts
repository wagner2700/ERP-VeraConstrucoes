import { Endereco } from "./IEndereco.interface";

export interface Cliente {
  tipoDocumento: 'cpf' | 'cnpj' | 'anonimo';
  documento?: string;
  nome?: string;
  email?: string;
  telefone?: string;
  endereco?: Endereco;
}