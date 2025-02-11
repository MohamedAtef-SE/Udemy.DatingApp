import { ChangeDetectorRef, Component, computed, inject, Input, input, InputSignal, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { IMember } from '../../../core/Models/IMember';
import { LikesService } from '../../../core/services/likes.service';
import { PaginationService } from '../../../core/services/pagination.service';
import { PaginatedResult } from '../../../core/Models/Pagination';
import { PresenceService } from '../../../core/services/presence.service';

@Component({
  selector: 'app-member-card',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css'
})
export class MemberCardComponent {
  
  _likesServices = inject(LikesService);
  _PresenceService = inject(PresenceService);
  _PaginationsService = inject(PaginationService);
  Member: InputSignal<IMember> = input.required<IMember>();
  hasLiked = computed(()=> this._likesServices.likeIds().includes(this.Member().id))
  isOnline = computed(()=> this._PresenceService.OnlineUsers().includes(this.Member().userName));


  toggleLike(){
    this._likesServices.toggleLike(this.Member().id).subscribe({
      next:_ => this._likesServices.getLikeIds()
    })
  }
}
