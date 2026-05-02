export interface InfEvento {
  id: string | null;
  tpAmb: number;
  verAplic: string;
  cOrgao: number;
  cStat: number;
  xMotivo: string;
  chNFe: string;
  tpEvento: number;
  xEvento: string | null;
  nSeqEvento: number;
  cOrgaoAutor: number | null;
  cnpjDest: string | null;
  cpfDest: string | null;
  emailDest: string | null;
  dhRegEvento: string;   // formato ISO, ex: "2026-03-14T15:07:01-03:00"
  proxydhRegEvento: string;
  nProt: string;
  chNFePend: string | null;
  signature: string | null;
}

