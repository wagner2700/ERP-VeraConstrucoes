import { InfEvento } from './IInfEvento.interface';

export interface RetornoEvento {
  versao: string;
  infEvento: InfEvento;
}


// Para uso em serviços, você pode tipar a resposta como um array:
export type RetornoEventoArray = RetornoEvento[];