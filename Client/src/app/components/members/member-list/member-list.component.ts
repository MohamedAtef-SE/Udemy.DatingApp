import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { MembersService } from '../../../core/services/members.service';
import { IMember } from '../../../core/Interfaces/IMember';
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
  Members:WritableSignal<IMember[]> = signal([]);

  ngOnInit(): void {
    this._MembersService.getAllMembers().subscribe({
      next:(res:IMember[])=>{
        this.Members.set(res);
      }
    });
  }

}