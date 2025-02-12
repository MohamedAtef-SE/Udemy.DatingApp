import { Component, inject } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { initFlowbite } from 'flowbite';
import { NgxSpinnerComponent } from 'ngx-spinner';
import { NavbarComponent } from "./components/navbar/navbar.component";
import { IUser } from './core/Models/Models';
import { AccountService } from './core/services/account.service';
import { LikesService } from './core/services/likes.service';
import { PresenceService } from './core/services/presence.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent,NgxSpinnerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  
  _AccountService = inject(AccountService);
  _LikesService = inject(LikesService);
  _PresenceService = inject(PresenceService);
  _Router = inject(Router);

  ngOnInit(): void {
    initFlowbite();
    this.getUser();

    this._Router.events.subscribe(event => {
      if(event instanceof NavigationEnd){
        window.scrollTo({top: 0, behavior: 'smooth'})
      }
    })
  }

  getUser():void{
    const userAsJSON = localStorage.getItem('DateAppUser');
    if(userAsJSON){
      const user : IUser = JSON.parse(userAsJSON);
      this._PresenceService.createHubConnection(user);
      this._AccountService.CurrentUser.set(user);
      this._LikesService.getLikeIds();
    }
  }

  

 
}