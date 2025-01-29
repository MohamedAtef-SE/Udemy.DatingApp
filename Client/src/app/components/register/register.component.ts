import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, OnInit, output, OutputEmitterRef } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { IRegisterForm, IRegisterResponse } from '../../core/Interfaces/Models';
import { AccountService } from '../../core/services/account.service';
import { InputTextComponent } from "../forms/input-text/input-text.component";
import { Router } from '@angular/router';
import { MembersService } from '../../core/services/members.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, InputTextComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  
  RegisterFG:FormGroup = new FormGroup({});
  _FormBuilder = inject(FormBuilder);
  _AccountService = inject(AccountService);
  _ToastrService = inject(ToastrService);
  CancelFromChild:OutputEmitterRef<boolean> = output<boolean>();
  _MemberService = inject(MembersService);
  ValidationErrors:string[] = []
  _Router = inject(Router);

  ngOnInit(): void {
    this.initializeFormGroup();
  }

  initializeFormGroup():void{
    this.RegisterFG = this._FormBuilder.group({
      gender:['male'],
      knownAs: [null,Validators.required],
      dateOfBirth:[null,Validators.required],
      city:[null,Validators.required],
      country:[null,Validators.required],
      username: [null,[Validators.required,Validators.minLength(4)]],
      password: [null,[Validators.required,Validators.minLength(6)]],
      confirmPassword: [null,[Validators.required,this.matchValues('password')]]
    })
    this.RegisterFG.controls['password'].valueChanges.subscribe({
      next:()=> this.RegisterFG.controls['confirmPassword'].updateValueAndValidity()
    })
  }

  matchValues(matchTo:string):ValidatorFn{
    return (control:AbstractControl) =>{
      return control.value === control.parent?.get(matchTo)?.value ? null : {mismatch: true}
    }
  }

  registerSubmit():void{
    
    if(this.RegisterFG.valid){
      this._AccountService.register(this.RegisterFG.value).subscribe({
        next: (res:IRegisterResponse) =>{
          this._ToastrService.success(`Welcome 😍 ${res.username},"DatingApp`);
          this._MemberService.loadMember();
          this._Router.navigate(['/members'])
        },
        error:(err)=>{
          this.ValidationErrors = err
        }
      })
    }
  }

  BackToHome():void{
    this.CancelFromChild.emit(false);
  }
}
