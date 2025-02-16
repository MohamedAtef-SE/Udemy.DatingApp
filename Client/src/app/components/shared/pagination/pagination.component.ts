import { Component, computed, inject, input, InputSignal, OnInit } from '@angular/core';
import { UserParams } from '../../../core/Models/userParams';
import { AccountService } from '../../../core/services/account.service';
import { MembersService } from '../../../core/services/members.service';
import { PaginationService } from '../../../core/services/pagination.service';
import { ActivatedRoute } from '@angular/router';
import { LikesService } from '../../../core/services/likes.service';
import { LikesParams } from '../../../core/Models/likesParams';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css'
})
export class PaginationComponent implements OnInit  {
  
  _MembersService = inject(MembersService);
  _PaginationService = inject(PaginationService);
  _AccountService = inject(AccountService);
  _ActivatedRoute = inject(ActivatedRoute);
  _LikesService = inject(LikesService);
  userParams!:UserParams
  likesParams!:LikesParams

  paginatedResult = computed(()=> this._PaginationService.paginatedResult());

  totalPages = computed(()=> this.paginatedResult()?.paginationHeader?.totalPages || 1);

  pageSize = computed(()=> this.paginatedResult()?.paginationHeader?.itemsPerPage || 10);

  currentPage = computed(()=>this.paginatedResult()?.paginationHeader?.currentPage);

  TotalPagesList = computed(()=> Array.from({length: this.totalPages()},(_,index)=> index+1))


  ngOnInit(): void {
    if(this._AccountService.CurrentUser() !== null){
      this.userParams = new UserParams(this._AccountService.CurrentUser()!);
      this.likesParams = new LikesParams();
    }
  }
  getPage(pageNumber: number) {
    if(this._ActivatedRoute.snapshot.url.toString().includes('lists')){
      this.likesParams.pageNumber = pageNumber;
      this._LikesService.getLikes(this.likesParams);
      console.log("list page")
      console.log(this._ActivatedRoute.snapshot.url.toString())
    }
    else if(this._ActivatedRoute.snapshot.url.toString().includes('members')){
      this.userParams.pageNumber = pageNumber;
      this._MembersService.getAllMembers(this.userParams);
      console.log("main-page")
      console.log(this._ActivatedRoute.snapshot.url.toString())
      
    }
    
    }
}
