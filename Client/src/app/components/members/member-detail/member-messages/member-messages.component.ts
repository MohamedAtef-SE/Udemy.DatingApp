import { AfterViewChecked, AfterViewInit, Component, computed, ElementRef, inject, OnDestroy, OnInit, Renderer2, signal, ViewChild, WritableSignal } from '@angular/core';
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
export class MemberMessagesComponent implements OnInit,AfterViewInit,OnDestroy {

  _MessageService = inject(MessageService);
  _ActivatedRoute = inject(ActivatedRoute);
  _AccountService = inject(AccountService);
  OtherUserName:WritableSignal<string> = signal('');
  MessageThread = computed(()=>this._MessageService.MessageThread());
  MessageContent:WritableSignal<string> = signal('');
  _ToastrService = inject(ToastrService);
  @ViewChild('messageForm') messageForm?:NgForm;
  @ViewChild('scrollMe') scrollContainer?:any


  ngOnInit(): void {
    this._ActivatedRoute.parent?.paramMap.subscribe({
      next: params => {
        const memberName = params.get("memberName");
        if(memberName){
          this.OtherUserName.set(memberName)
          this.loadThreadMessage();
        } 
      }
    });
  }

  sendMessage(){
    this._MessageService.sendMessage(this.OtherUserName(),this.MessageContent()).then(()=>{
      this.messageForm?.reset();
      this.scrollToBottom();
    })
  }

  ngAfterViewInit() {
    setTimeout(()=>{
      this.scrollToBottom();
    },500)
  }

  private scrollToBottom(){
    if(this.scrollContainer){
      console.log("scroll to bottom............")
      this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
    }
  }

  loadThreadMessage():void{
    const user = this._AccountService.CurrentUser();
    if(!user) return;
    this._MessageService.createHubConnection(user,this.OtherUserName());
  }

  ngOnDestroy(): void {
    this._MessageService.stopHubConnection();
  }
}
