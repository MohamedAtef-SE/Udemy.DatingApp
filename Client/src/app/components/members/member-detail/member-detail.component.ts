import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit {
  _ActivatedRoute = inject(ActivatedRoute);
  MamberId:WritableSignal<string | null> = signal(null);

  ngOnInit():void{
    this._ActivatedRoute.paramMap.subscribe({
  next:(params)=>{
    const memberId =params.get('id');
    console.log(`MemberID: ${memberId}`)
    this.MamberId.set(memberId);
  }
})
  }


}
