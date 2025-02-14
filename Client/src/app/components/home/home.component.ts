import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ILoginForm, IUser } from '../../core/Models/Models';
import { AccountService } from '../../core/services/account.service';
import { ToastrService } from 'ngx-toastr';
import { MembersService } from '../../core/services/members.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [ReactiveFormsModule,RouterLink],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  _AccountService = inject(AccountService);
  _ToastRService = inject(ToastrService);
  _MemberService = inject(MembersService);
  _Router = inject(Router);

  loginGroup:FormGroup = new FormBuilder().group( {
    username: [null,[Validators.required,Validators.minLength(4)]],
    password: [null,[Validators.required,Validators.minLength(6)]]
  })


loginSubmit() {
  if(this.loginGroup.invalid){
    this._ToastRService.error("Invalid login","Hommis");
    return;
  }
  
  const login:ILoginForm = {
    password: this.loginGroup.get('password')?.value,
    username: this.loginGroup.get('username')?.value
  }

this._AccountService.login(login).subscribe({
  next:(res:IUser)=>{
    this._Router.navigateByUrl('/members');
    this._ToastRService.success(`Welcome, ${res.knownAs}`,"Homies");
    this._MemberService.loadMember();
  }
})
}

 
  RegisterMode:WritableSignal<boolean> = signal(false);

  registerToggle(event:boolean = true):void {
    this.RegisterMode.set(event);
  }
}
