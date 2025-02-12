import { Component, computed, inject, Signal, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IMember, IMemberUpdateForm } from '../../../../core/Models/IMember';
import { MembersService } from '../../../../core/services/members.service';

@Component({
  selector: 'app-edit-profile',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './edit-profile.component.html',
  styleUrl: './edit-profile.component.css'
})

export class EditProfileComponent {
  
  Member:Signal<IMember> = computed(()=> this._MembersService.Member())
  _MembersService = inject(MembersService);
  _ToastrService = inject(ToastrService);
  _Router = inject(Router);

  @ViewChild('editForm') editForm? : NgForm
  


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
        this._ToastrService.success("Profile updated successfully")
        this._Router.navigate([`members/${this.Member().userName}`])
      }
    })
    
    this.editForm?.reset(this.Member());
    }
    
}

