import { Component, inject, Input, OnInit, signal, WritableSignal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IMember } from '../../../../core/Interfaces/IMember';

interface IAbout {
  KnownAs: string,
  Introduction: string
}
@Component({
  selector: 'app-about',
  standalone: true,
  imports: [],
  templateUrl: './about.component.html',
  styleUrl: './about.component.css'
})
export class AboutComponent implements OnInit {
  

  KnownAs:WritableSignal<string | null> = signal(null);
  Introduction:WritableSignal<string | null> = signal(null);
  _ActivatedRoute = inject(ActivatedRoute);

  ngOnInit(): void {
    this._ActivatedRoute.queryParamMap.subscribe({
    next:(params)=>{
      this.KnownAs.set(params.get('KnownAs'));
      this.Introduction.set(params.get('Introduction'));
      }
    })
    
  }
}
