import { DatePipe } from '@angular/common';
import { Component, computed, effect, inject, OnInit, Signal, signal, WritableSignal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { IMember } from '../../../core/Interfaces/IMember';
import { AccountService } from '../../../core/services/account.service';
import { MembersService } from '../../../core/services/members.service';

@Component({
  selector: 'app-member-edit',
  standalone: true,
  imports: [DatePipe, RouterLink, RouterLinkActive,RouterOutlet],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css'
})
export class MemberEditComponent implements OnInit {
  
  Member:Signal<IMember> = computed(()=> this._MembersService.Member());
  EditToggle: WritableSignal<string> = signal("EditProfile")
  _AccountService = inject(AccountService);
  _MembersService = inject(MembersService);
  _Router =inject(Router);


  
  ngOnInit(): void {
    this._MembersService.loadMember();
    console.log("Hello From member-edit component")
  }

  EditProfile() {
    this.EditToggle.set("EditProfile")
    }

  EditPhotos() {
    this.EditToggle.set("EditPhotos")
    }
}
