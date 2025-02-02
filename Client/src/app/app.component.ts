import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { initFlowbite } from 'flowbite';
import { NavbarComponent } from "./components/navbar/navbar.component";
import { AccountService } from './core/services/account.service';
import { NgxSpinnerComponent } from 'ngx-spinner';
import { ICurrentUser } from './core/Models/Models';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent,NgxSpinnerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {

  _AccountService = inject(AccountService);

  ngOnInit(): void {
    initFlowbite();
    this.getUser();
  }

  getUser():void{
    const userAsJSON = localStorage.getItem('DateAppUserToken');
    if(userAsJSON){
      const user : ICurrentUser = JSON.parse(userAsJSON);
      this._AccountService.CurrentUser.set(user);
    }
  }
}