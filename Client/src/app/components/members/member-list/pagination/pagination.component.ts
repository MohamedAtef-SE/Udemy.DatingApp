import { Component, computed, inject, OnInit } from '@angular/core';
import { UserParams } from '../../../../core/Models/userParams';
import { AccountService } from '../../../../core/services/account.service';
import { MembersService } from '../../../../core/services/members.service';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css'
})
export class PaginationComponent implements OnInit  {
  
  _MembersService = inject(MembersService);
  _AccountService = inject(AccountService);
  userParams!:UserParams

  paginatedResult = computed(()=> this._MembersService.paginatedResult());

  totalPages = computed(()=> this.paginatedResult()?.paginationHeader?.totalPages || 1);

  pageSize = computed(()=> this.paginatedResult()?.paginationHeader?.itemsPerPage || 10);

  currentPage = computed(()=>this.paginatedResult()?.paginationHeader?.currentPage);

  TotalPagesList = computed(()=> Array.from({length: this.totalPages()},(_,index)=> index+1))


  ngOnInit(): void {
    if(this._AccountService.CurrentUser() !== null){
      this.userParams = new UserParams(this._AccountService.CurrentUser()!);
    }
  }
  getPage(pageNumber: number) {
    this.userParams.pageNumber = pageNumber;
    this._MembersService.getAllMembers(this.userParams);
    }
}
