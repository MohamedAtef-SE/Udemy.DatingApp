import { Component, computed, inject, OnInit, Signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AnimationItem } from 'lottie-web';
import { AnimationOptions, LottieComponent, LottieModule } from 'ngx-lottie';
import { IMember } from '../../../core/Models/IMember';
import { UserParams } from '../../../core/Models/userParams';
import { AccountService } from '../../../core/services/account.service';
import { MembersService } from '../../../core/services/members.service';
import { PaginationService } from '../../../core/services/pagination.service';
import { PaginationComponent } from "../../shared/pagination/pagination.component";
import { MemberCardComponent } from '../member-card/member-card.component';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent,PaginationComponent,FormsModule,LottieModule,LottieComponent],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {
  
  _MembersService = inject(MembersService);
  _AccountService = inject(AccountService);
  _PaginationService = inject(PaginationService);
  userParams!:UserParams;
  Members:Signal<IMember[] | undefined> = computed(()=> this._PaginationService.paginatedResult()?.items);
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
    
    this._MembersService.getAllMembers(this.userParams);
  }

  filterationSubmit() {
    this._MembersService.pickedUserparams.set(this.userParams);
    this._MembersService.getAllMembers(this.userParams);
   }
   
  resetFilter(){
    if(this._AccountService.CurrentUser()){
      this.userParams = new UserParams(this._AccountService.CurrentUser()!);
    }
    this._MembersService.getAllMembers(this.userParams);
  }

  options: AnimationOptions = {
      path: '/assets/svg/desert.json',
    };
  
    animationCreated(animationItem: AnimationItem): void {
      console.log(animationItem);
    }
  
}