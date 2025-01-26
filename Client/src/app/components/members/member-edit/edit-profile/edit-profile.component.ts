import { Component, HostListener, inject, OnInit, signal, ViewChild, WritableSignal } from '@angular/core';
import { IMember, IMemberUpdateForm } from '../../../../core/Interfaces/IMember';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { MembersService } from '../../../../core/services/members.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-edit-profile',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './edit-profile.component.html',
  styleUrl: './edit-profile.component.css'
})

export class EditProfileComponent implements OnInit {

  Member:WritableSignal<IMember> = signal({} as IMember);
  _MembersService = inject(MembersService);
  _ToastrService = inject(ToastrService);
  _Router = inject(Router);

  @ViewChild('editForm') editForm? : NgForm
  
  ngOnInit(): void {
    const member: IMember = history.state.data.member;
    if(!member) return;
      this.Member.set(member);
  }

  @HostListener('window:beforeunload',['$event']) notify(event:any){
    if(this.editForm?.dirty){
      event.returnValue = true;
    }
    else{
      event.returnValue = false;
    }
  }

  updateSubmit() {
    const updatedMember : IMemberUpdateForm = {
      City: this.Member().city,
      Country: this.Member().country,
      Interests: this.Member().interests,
      Introduction: this.Member().introduction,
      LookingFor: this.Member().lookingFor
    }
    this._MembersService.updateMember(updatedMember).subscribe({
      next:(res)=>{
        console.log(res)
        this._ToastrService.success("Profile updated successfully")
        this._Router.navigate([`members/${this.Member().userName}`])
      }
    })
    
    this.editForm?.reset(this.Member());
    }
    
}

