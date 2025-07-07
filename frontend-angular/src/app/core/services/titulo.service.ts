import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface ParcelaDto {
  numeroParcela: number;
  valor: number;
  vencimento: string;
}

export interface TituloComParcelasDto {
  numero: string;
  nomeDevedor: string;
  cpf: string;
  jurosMensal: number;
  multaPercentual: number;
  parcelas: ParcelaDto[];
}

@Injectable({
  providedIn: 'root'
})
export class TituloService {
  private apiUrl = 'http://localhost:5136/titulos/com-parcelas';

  constructor(private http: HttpClient) {}

  listarComParcelas(cpf?: string, numeroTitulo?: string, page: number = 1, pageSize: number = 10): Observable<TituloComParcelasDto[]> {
    let params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    if (cpf) params = params.set('cpf', cpf);
    if (numeroTitulo) params = params.set('numeroTitulo', numeroTitulo);

    return this.http.get<TituloComParcelasDto[]>(this.apiUrl, { params });
  }
}