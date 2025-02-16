import { Component, computed, inject, OnInit, Signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AnimationOptions, LottieComponent } from 'ngx-lottie';
import { IMember } from '../../core/Models/IMember';
import { LikesParams } from '../../core/Models/likesParams';
import { LikesService } from '../../core/services/likes.service';
import { PaginationService } from '../../core/services/pagination.service';
import { MemberCardComponent } from '../members/member-card/member-card.component';
import { PaginationComponent } from '../shared/pagination/pagination.component';
import { AnimationItem } from 'lottie-web';

@Component({
  selector: 'app-lists',
  standalone: true,
  imports: [MemberCardComponent,FormsModule,PaginationComponent,LottieComponent],
  templateUrl: './lists.component.html',
  styleUrl: './lists.component.css'
})
export class ListsComponent implements OnInit {

  private readonly _LikesService = inject(LikesService);
  private readonly _PaginationService = inject(PaginationService);
  Members:Signal<IMember[] | undefined> = computed(()=> this._PaginationService.paginatedResult()?.items)
  likesParams = new LikesParams()

  ngOnInit(): void {
    this._PaginationService.paginatedResult.set(null);
      this.loadMembers();
  }

  loadMembers=()=>{
    this._LikesService.getLikes(this.likesParams);
    console.log('list loaded again...')
  }

  setPredicate(predicate:string){
    this.likesParams.predicate = predicate;
    this.loadMembers();
  }
  getTitle():string{
    switch(this.likesParams.predicate)
    {
      case 'liked': return "Members you like";
      case 'likedBy': return "Members who like you";
      default: return "Mutual";
    }
  }
    options: AnimationOptions = {
        path: '/assets/svg/desert.json',
      };
    
      animationCreated(animationItem: AnimationItem): void {
        console.log(animationItem);
      }
    
}