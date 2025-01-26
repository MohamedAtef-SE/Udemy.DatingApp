import { NgClass } from '@angular/common';
import { AfterViewChecked, AfterViewInit, Component, Input, signal, WritableSignal } from '@angular/core';


interface IBtnClass{
  bg: string,
  color: string,
  bgHover?: string
}

@Component({
  selector: 'app-main-button',
  standalone: true,
  imports: [],
  templateUrl: './main-button.component.html',
  styleUrl: './main-button.component.css'
})

export class MainButtonComponent  {
  
  @Input() Type: string = "button";

  @Input() myClass : IBtnClass = {
    'bg': "bg-red-500",
    'color': "text-white",
    'bgHover': "hover:bg-sky-700"
  }
isPrimary: any;

}
