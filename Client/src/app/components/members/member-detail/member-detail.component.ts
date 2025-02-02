import { DatePipe } from '@angular/common';
import { Component, HostListener, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { IMember } from '../../../core/Models/IMember';
import { IPhoto } from '../../../core/Models/IPhoto';
import { MembersService } from '../../../core/services/members.service';
import { TimeagoModule } from 'ngx-timeago';

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

  ngOnInit():void{
    this.UserName.set(this._ActivatedRoute.snapshot.paramMap.get('username'));
    if(this.UserName()){
      this._MembersService.getMember(this.UserName()!).subscribe({
        next:(res:IMember)=>{
          this.Member.set(res);
        }
      })
    }
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
