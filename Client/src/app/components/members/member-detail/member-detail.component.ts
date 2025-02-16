import { DatePipe } from '@angular/common';
import { Component, computed, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { ImageItem } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { ToastrService } from 'ngx-toastr';
import { IMember } from '../../../core/Models/IMember';
import { IPhoto } from '../../../core/Models/IPhoto';
import { MembersService } from '../../../core/services/members.service';
import { PresenceService } from '../../../core/services/presence.service';
import { state } from '@angular/animations';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [RouterLinkActive,RouterLink,RouterOutlet,TimeagoModule,DatePipe],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})

export class MemberDetailComponent implements OnInit {

  _ActivatedRoute = inject(ActivatedRoute);
  _MembersService = inject(MembersService);
  _PresenceService = inject(PresenceService);
  _ToastrService = inject(ToastrService);
  _Router = inject(Router);
  UserName:WritableSignal<string | null> = signal(null);
  Member: WritableSignal<IMember> = signal({} as IMember);
  Clicked:WritableSignal<boolean> = signal(false);
  isOnline = computed(()=> this._PresenceService.OnlineUsers().includes(this.Member().userName))
  Images:ImageItem[] = []
  SerializedImages:string = "";

  ngOnInit():void{
    // From Resolver
    this._ActivatedRoute.data.subscribe({
      next: data => {
        this.Member.set(data['member']);
        this.Member() && this.Member().photos.map(p => {
          this.Images.push(new ImageItem({src: p.url, thumb: p.url}))
        } )
        this.SerializedImages =  JSON.stringify(this.Images);
      }
    })
  }


  setPhotos(photos: IPhoto[]) {
    this.Clicked.set(true);
    this._Router.navigateByUrl(`members/${this.UserName()}/photos`,{state: {data: photos}})
  }

}
