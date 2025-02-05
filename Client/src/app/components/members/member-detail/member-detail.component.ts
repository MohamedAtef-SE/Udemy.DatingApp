import { DatePipe } from '@angular/common';
import { Component, HostListener, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { ImageItem } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { IMember } from '../../../core/Models/IMember';
import { IPhoto } from '../../../core/Models/IPhoto';
import { MembersService } from '../../../core/services/members.service';

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
  _Router = inject(Router);
  UserName:WritableSignal<string | null> = signal(null);
  Member: WritableSignal<IMember> = signal({} as IMember);
  Clicked:WritableSignal<boolean> = signal(false);
  Images:ImageItem[] = []
  SerializedImages:string = "";

  ngOnInit():void{
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

// Display Default Message if user pressed on BACK Browser Button with no tab selected yet!!
@HostListener('window:popstate',['$event']) onPopState(event:any){
  const UrlToArray = event.currentTarget.location.href.split('/');
  const URLPathTail = event.currentTarget.location.href.split('/')[UrlToArray.length - 1];
  if(URLPathTail == this.UserName()){
    this.Clicked.set(false);
  }
  
}

  setPhotos(photos: IPhoto[]) {
    this.Clicked.set(true);
    this._Router.navigateByUrl(`members/${this.UserName()}/photos`,{state: {data: photos}})
  }

}
