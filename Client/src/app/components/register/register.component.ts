import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, output, OutputEmitterRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { IRegisterForm, IRegisterResponse } from '../../core/Interfaces/Models';
import { AccountService } from '../../core/services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {

  model:IRegisterForm  = {} as IRegisterForm;
  _AccountService = inject(AccountService);
  _ToastrService = inject(ToastrService);
  CancelFromChild:OutputEmitterRef<boolean> = output<boolean>();

  registerSubmit():void{
    this._AccountService.register(this.model).subscribe({
      next: (res:IRegisterResponse) =>{
        this._ToastrService.success(`Welcome 😍 ${res.username},"DatingApp`);
        this.BackToHome();
      },
      error:(err:HttpErrorResponse)=>{
       this._ToastrService.error(err.message,"DatingApp 😬")
      },
      complete:()=>{
        console.log('Register Completed...')
      }
    })
  }

  BackToHome():void{
    this.CancelFromChild.emit(false);
  }
}
