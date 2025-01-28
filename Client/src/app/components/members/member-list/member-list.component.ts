import { Component, computed, inject, OnInit, Signal } from '@angular/core';
import { IMember } from '../../../core/Interfaces/IMember';
import { MembersService } from '../../../core/services/members.service';
import { MemberCardComponent } from '../member-card/member-card.component';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {
  
  _MembersService = inject(MembersService);
  Members:Signal<IMember[]> = computed(()=> this._MembersService.Members());

  ngOnInit(): void {
    if(this._MembersService.Members().length === 0){
      this._MembersService.getAllMembers();
    }
  }
}