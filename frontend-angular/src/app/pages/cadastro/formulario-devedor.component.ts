import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  FormArray,
  Validators,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { DevedorService } from '../../core/services/devedor.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, MAT_DATE_LOCALE, MAT_DATE_FORMATS } from '@angular/material/core';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';

// ✅ Validador de CPF
function validarCPF(cpf: string): boolean {
  cpf = cpf.replace(/[^\d]+/g, '');
  if (cpf.length !== 11 || /^(\d)\1+$/.test(cpf)) return false;

  let soma = 0;
  for (let i = 0; i < 9; i++) soma += +cpf[i] * (10 - i);
  let resto = (soma * 10) % 11;
  if (resto === 10 || resto === 11) resto = 0;
  if (resto !== +cpf[9]) return false;

  soma = 0;
  for (let i = 0; i < 10; i++) soma += +cpf[i] * (11 - i);
  resto = (soma * 10) % 11;
  if (resto === 10 || resto === 11) resto = 0;
  return resto === +cpf[10];
}

function cpfValidator(control: AbstractControl): ValidationErrors | null {
  const value = control.value;
  if (!value) return null;
  return validarCPF(value) ? null : { cpfInvalido: true };
}
function formatarDataParaApi(data: Date): string {
  const ano = data.getFullYear();
  const mes = String(data.getMonth() + 1).padStart(2, '0');
  const dia = String(data.getDate()).padStart(2, '0');
  return `${ano}${mes}${dia}`; // Resultado: "20250706"
}

// ✅ Conversor de valores mascarados para número
function parseDecimal(valor: string | number): number {
  if (typeof valor === 'number') return valor;
  if (!valor) return 0;

  const limpo = valor
    .toString()
    .replace(/[^\d,.-]/g, '')
    .replace(/\./g, '')
    .replace(',', '.');

  return parseFloat(limpo);
}

@Component({
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatDatepickerModule,
    MatNativeDateModule,
    NgxMaskDirective,
    NgxMaskPipe
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'pt-BR' },
    {
      provide: MAT_DATE_FORMATS,
      useValue: {
        parse: { dateInput: 'DD/MM/YYYY' },
        display: {
          dateInput: 'dd/MM/yyyy',
          monthYearLabel: 'MMMM yyyy',
          dateA11yLabel: 'LL',
          monthYearA11yLabel: 'MMMM yyyy',
        },
      },
    }
  ],
  templateUrl: './formulario-devedor.component.html',
  styleUrls: ['./formulario-devedor.component.scss']
})
export class FormularioDevedorComponent {
  devedorForm: FormGroup;

  constructor(private fb: FormBuilder, private devedorService: DevedorService) {
    this.devedorForm = this.fb.group({
      numero: ['', Validators.required],
      nomeDevedor: ['', Validators.required],
      cpf: ['', [Validators.required, cpfValidator]],
      jurosMensal: [null, Validators.required],
      multaPercentual: [null, Validators.required],
      parcelas: this.fb.array([])
    });

    this.adicionarParcela();
  }

  get parcelas(): FormArray {
    return this.devedorForm.get('parcelas') as FormArray;
  }

  adicionarParcela(): void {
    const numeroAtual = this.parcelas.length + 1;
    this.parcelas.push(this.fb.group({
      numeroParcela: [numeroAtual, Validators.required],
      vencimento: [null, [Validators.required, this.dataFuturaValidator]],
      valor: [0, Validators.required]
    }));
  }

  removerParcela(index: number): void {
    this.parcelas.removeAt(index);
    this.parcelas.controls.forEach((grupo, i) => {
      grupo.get('numeroParcela')?.setValue(i + 1);
    });
  }

  dataFuturaValidator(control: AbstractControl): ValidationErrors | null {
    const data = new Date(control.value);
    const hoje = new Date();
    hoje.setHours(0, 0, 0, 0);
    return data < hoje ? { dataPassada: true } : null;
  }

  onSubmit(): void {
    console.log('Formulário enviado:', this.devedorForm.value); // 👈 Aqui
    if (this.devedorForm.invalid) {
      this.devedorForm.markAllAsTouched();
      return;
    }

    const formValue = this.devedorForm.value;

    const payload = {
      ...formValue,
      jurosMensal: parseDecimal(formValue.jurosMensal),
      multaPercentual: parseDecimal(formValue.multaPercentual),
      parcelas: formValue.parcelas.map((p: any) => ({
        ...p,
        valor: parseDecimal(p.valor),
        vencimento: new Date(p.vencimento).toISOString() //
      }))
    };

    
    console.log('Payload enviado para a API:', payload); // 👈 Aqui


    this.devedorService.salvarDevedor(payload).subscribe({
      next: () => alert('Dados enviados com sucesso!'),
      error: err => alert('Erro ao enviar: ' + err.message)
    });
  }
}