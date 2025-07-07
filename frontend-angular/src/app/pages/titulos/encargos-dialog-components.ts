import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-encargos-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule],
  template: `
    <h2 mat-dialog-title>Encargos Calculados</h2>
    <mat-dialog-content>
      <p><strong>Total das parcelas:</strong> R$ {{ data.totalParcelas.toFixed(2) }}</p>
      <p><strong>Juros por atraso:</strong> R$ {{ data.totalJuros.toFixed(2) }}</p>
      <p><strong>Multa:</strong> R$ {{ data.multa.toFixed(2) }}</p>
      <p><strong>Total final:</strong> R$ {{ data.totalFinal.toFixed(2) }}</p>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Fechar</button>
    </mat-dialog-actions>
  `
})
export class EncargosDialogComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: {
    totalParcelas: number;
    totalJuros: number;
    multa: number;
    totalFinal: number;
  }) {}
}