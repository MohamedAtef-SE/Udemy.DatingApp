import { Component, inject, OnInit, signal, ViewChild, WritableSignal } from '@angular/core';
import { ICurrentUser } from '../../../core/Models/Models';
import { AdminService } from '../../../core/services/admin.service';
import { RoleModalComponent } from "../../modals/role-modal/role-modal.component";

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [RoleModalComponent],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css'
})
export class UserManagementComponent implements OnInit {

  
  _AdminService = inject(AdminService);
  Users:WritableSignal<ICurrentUser[]> = signal([]);
  @ViewChild(RoleModalComponent) modal: RoleModalComponent = new RoleModalComponent();

  ngOnInit(): void {
    this.getUsersWithRoles();
   
  }

  getUpdatedUser(event: ICurrentUser) {
    this.Users.update(prev => {
      prev.map(user => {
        if(user.username === event.username){
          user.roles = event.roles
        }
        return user;
      })
      return prev})  
  }
    
  openModal(user:ICurrentUser){
    this.modal.show(user);
  }
  
  getUsersWithRoles():void{
    console.log('Getting users')
    this._AdminService.getUsersWithRoles().subscribe({
      next:(res:ICurrentUser[])=>{
        this.Users.set(res);
      }
    })
  }

}
