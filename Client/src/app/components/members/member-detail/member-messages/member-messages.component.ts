import { Component, computed, inject, OnDestroy, OnInit, signal, ViewChild, WritableSignal } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TimeagoModule } from 'ngx-timeago';
import { AccountService } from '../../../../core/services/account.service';
import { MessageService } from '../../../../core/services/message.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [TimeagoModule,FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css'
})
export class MemberMessagesComponent implements OnInit, OnDestroy {

  _MessageService = inject(MessageService);
  _ActivatedRoute = inject(ActivatedRoute);
  _AccountService = inject(AccountService);
  OtherUserName:WritableSignal<string> = signal('');
  MessageThread = computed(()=>this._MessageService.MessageThread());
  MessageContent:WritableSignal<string> = signal('');
  _ToastrService = inject(ToastrService);
  @ViewChild('messageForm') messageForm?:NgForm;




  ngOnInit(): void {
    this._ActivatedRoute.parent?.paramMap.subscribe({
      next: params => {
        const memberName = params.get("memberName");
        console.log("RecipientNAme: ",memberName)
        if(memberName){
          this.OtherUserName.set(memberName)
          this.loadThreadMessage();
        } 
      }
    });
  }

  loadThreadMessage():void{
    const user = this._AccountService.CurrentUser();
    if(!user) return;
    this._MessageService.createHubConnection(user,this.OtherUserName());
  }

  sendMessage(){
    this._MessageService.sendMessage(this.OtherUserName(),this.MessageContent()).then(()=>{
      this.messageForm?.reset();
    })
  }

  ngOnDestroy(): void {
    this._MessageService.stopHubConnection();
  }
}
