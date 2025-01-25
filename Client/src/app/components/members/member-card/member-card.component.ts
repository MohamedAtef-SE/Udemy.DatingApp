import { Component, input, InputSignal, OnInit, Signal, signal, WritableSignal } from '@angular/core';
import { IMember } from '../../../core/Interfaces/IMember';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-member-card',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css'
})
export class MemberCardComponent {
  
  Member: InputSignal<IMember> = input.required<IMember>();


}
