import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-interests',
  standalone: true,
  imports: [],
  templateUrl: './interests.component.html',
  styleUrl: './interests.component.css'
})
export class InterestsComponent implements OnInit {

  _ActivatedRoute = inject(ActivatedRoute);
  Interests:WritableSignal<string | null> = signal(null);
  LookingFor:WritableSignal<string | null> = signal(null);
 
  ngOnInit():void{
   this._ActivatedRoute.queryParamMap.subscribe(params=> {
    this.Interests.set(params.get('Interests'));
    this.LookingFor.set(params.get('LookingFor'));
   })
    
  }


}
