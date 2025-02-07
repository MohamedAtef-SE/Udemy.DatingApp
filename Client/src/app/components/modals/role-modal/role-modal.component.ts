import { Component, inject, input, InputSignal, signal, WritableSignal, computed, OnInit, Signal, Input, Output, EventEmitter } from '@angular/core';
import { ICurrentUser } from '../../../core/Models/Models';
import { AccountService } from '../../../core/services/account.service';
import { AdminService } from '../../../core/services/admin.service';


@Component({
  selector: 'app-role-modal',
  standalone: true,
  imports: [],
  templateUrl: './role-modal.component.html',
  styleUrl: './role-modal.component.css'
})
export class RoleModalComponent {
  
  _AccountService = inject(AccountService);
  _AdminService = inject(AdminService);
  User: WritableSignal<ICurrentUser> = signal({} as ICurrentUser);
  username = '';
  hideModal:boolean = true;
  title:string = '';
  availableRoles:string[] =  ['Admin','Moderator','Member'];
  selectedRoles:WritableSignal<string[]> = signal([]);
  @Output() UpdatedUser:EventEmitter<ICurrentUser> = new EventEmitter();
  updateChecked(checkedValue:string){
    if(this.selectedRoles().includes(checkedValue)){
      this.selectedRoles.update(prevValue=>  prevValue.filter(r => r !== checkedValue))
    }else{
      this.selectedRoles.update(prevValue => [...prevValue,checkedValue])
    }
  }

  updateRoles(){
    this.hide();
    this._AdminService.updateUserRoles(this.User().username,this.selectedRoles()).subscribe({
    next:(res:string[])=>{
      this.User.update(prev => {
        return {
          ...prev,
          roles: res
        }
      })
    this.UpdatedUser.emit(this.User())
    }
  })
  }

  show = (user:ICurrentUser)=> {
    this.User.set(user);
    if(user.roles !== undefined){
      this.selectedRoles.set(user.roles)
    }
    this.hideModal = false;
  }

  hide = ()=>{
    this.hideModal = true;
  }
}
