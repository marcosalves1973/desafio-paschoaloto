import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { TituloService, TituloComParcelasDto } from '../../core/services/titulo.service';
import { EncargosDialogComponent } from './encargos-dialog-components'; // certifique-se de ajustar o caminho

@Component({
  selector: 'app-titulos',
  standalone: true,
  imports: [
    CommonModule,
    MatExpansionModule,
    MatTableModule,
    MatDialogModule,
    EncargosDialogComponent
  ],
  templateUrl: './titulos.component.html',
  styleUrls: ['./titulos.component.scss']
})
export class TitulosComponent implements OnInit {
  titulos: TituloComParcelasDto[] = [];
  page = 1;
  pageSize = 30;

  constructor(
    private tituloService: TituloService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.carregarTitulos();
  }

  carregarTitulos(): void {
    this.tituloService
      .listarComParcelas(undefined, undefined, this.page, this.pageSize)
      .subscribe((data: TituloComParcelasDto[]) => this.titulos = data);
  }

  calcularEncargos(titulo: TituloComParcelasDto): void {
    const hoje = new Date();
    let totalParcelas = 0;
    let totalJuros = 0;

    for (const parcela of titulo.parcelas) {
      const vencimento = new Date(parcela.vencimento);
      const mesesAtraso = this.calcularMesesAtraso(vencimento, hoje);

      const juros = parcela.valor * (titulo.jurosMensal / 100) * mesesAtraso;
      totalJuros += juros;
      totalParcelas += parcela.valor;
    }

    const multa = totalParcelas * (titulo.multaPercentual / 100);
    const totalFinal = totalParcelas + totalJuros + multa;

    const resultado = {
      totalParcelas,
      totalJuros,
      multa,
      totalFinal
    };

    this.exibirResultado(resultado);
  }

  calcularMesesAtraso(vencimento: Date, hoje: Date): number {
    if (vencimento >= hoje) return 0;

    const anos = hoje.getFullYear() - vencimento.getFullYear();
    const meses = hoje.getMonth() - vencimento.getMonth();
    const total = anos * 12 + meses;

    return hoje.getDate() < vencimento.getDate() ? total - 1 : total;
  }

  exibirResultado(resultado: {
    totalParcelas: number;
    totalJuros: number;
    multa: number;
    totalFinal: number;
  }): void {
    this.dialog.open(EncargosDialogComponent, {
      data: resultado,
      width: '400px'
    });
  }
}