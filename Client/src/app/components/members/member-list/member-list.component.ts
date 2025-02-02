import { Component, computed, inject, OnInit, Signal } from '@angular/core';
import { IMember } from '../../../core/Models/IMember';
import { MembersService } from '../../../core/services/members.service';
import { MemberCardComponent } from '../member-card/member-card.component';
import { PaginationComponent } from "./pagination/pagination.component";
import { AccountService } from '../../../core/services/account.service';
import { UserParams } from '../../../core/Models/userParams';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent, PaginationComponent,FormsModule],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {

  _MembersService = inject(MembersService);
  _AccountService = inject(AccountService);
  userParams!:UserParams;
  Members:Signal<IMember[] | undefined> = computed(()=> this._MembersService.paginatedResult()?.items);
  genderList = [{value:'male',display:'Males'},{value:'female',display:'Females'}]
  sortingList = 
  [
    {value:'created',display: 'newest'},
  ];


  ngOnInit(): void {
    if(this._AccountService.CurrentUser()){
      this.userParams = new UserParams(this._AccountService.CurrentUser()!);
    }
    const pickedUserParams = this._MembersService.pickedUserparams();
    if(pickedUserParams){
      this.userParams = pickedUserParams;
    }
    
    if(this._MembersService.paginatedResult() === null){
      this._MembersService.getAllMembers(this.userParams);
    }
  }

  filterationSubmit(userParams: UserParams) {
    this._MembersService.pickedUserparams.set(userParams);
    this._MembersService.getAllMembers(userParams);
   }
   
  resetFilter(){
    if(this._AccountService.CurrentUser()){
      this.userParams = new UserParams(this._AccountService.CurrentUser()!);
    }
    this._MembersService.getAllMembers(this.userParams);
  }
}