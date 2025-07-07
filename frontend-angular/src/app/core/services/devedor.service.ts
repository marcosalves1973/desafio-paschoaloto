import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Parcela {
  numeroParcela: number;
  vencimento: string;
  valor: number;
}

export interface Devedor {
  numero: string;
  nomeDevedor: string;
  cpf: string;
  jurosMensal: number;
  multaPercentual: number;
  parcelas: Parcela[];
}

@Injectable({
  providedIn: 'root'
})
export class DevedorService {
  private apiUrl = 'http://localhost:5136/titulos'; 

  constructor(private http: HttpClient) {}

  salvarDevedor(devedor: Devedor): Observable<any> {
    return this.http.post(this.apiUrl, devedor);
  }
}