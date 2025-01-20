import { Component, signal, WritableSignal } from '@angular/core';
import { MainButtonComponent } from "../buttons/main-button/main-button.component";
import { RegisterComponent } from "../register/register.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [MainButtonComponent, RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

 
  RegisterMode:WritableSignal<boolean> = signal(false);

  registerToggle(event:boolean = true):void {
    this.RegisterMode.set(event);
  }
}
