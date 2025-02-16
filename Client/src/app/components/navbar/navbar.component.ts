import { Component, computed, inject, OnInit, Signal, signal, WritableSignal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HasRoleDirective } from '../../core/directives/has-role.directive';
import { ILoginForm, IUser } from '../../core/Models/Models';
import { MembersService } from '../../core/services/members.service';
import { AccountService } from './../../core/services/account.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [FormsModule, RouterLink,RouterLinkActive,HasRoleDirective],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit  {

  _Router = inject(Router);
  _ToastrService = inject(ToastrService);
  _MembersService = inject(MembersService);
  model:WritableSignal<ILoginForm> = signal({} as ILoginForm);
  private readonly _AccountService = inject(AccountService);
  UserData:Signal<IUser | null> = computed(()=> this._AccountService.CurrentUser())
  IsLoggedIn:Signal<boolean> = computed(()=> this._AccountService.CurrentUser() !== null);
  showDropdown:WritableSignal<boolean> = signal(false);
 
  ngOnInit(): void {
    this._MembersService.loadMember();
  }

 
  toggle():void{
    this.showDropdown.update(prev => !prev);
  }

logout():void{
  this._AccountService.logout();
  this.toggle();
  this._ToastrService.info(`we miss you already 😔 `,"Homies")
  this._Router.navigateByUrl('/');

}

}
