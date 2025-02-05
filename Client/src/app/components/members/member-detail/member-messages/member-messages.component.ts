import { Component, ElementRef, inject, input, InputSignal, OnInit, signal, ViewChild, WritableSignal } from '@angular/core';
import { IMessage } from '../../../../core/Models/Message';
import { MessageService } from '../../../../core/services/message.service';
import { TimeagoModule } from 'ngx-timeago';
import { ActivatedRoute } from '@angular/router';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [TimeagoModule,FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css'
})
export class MemberMessagesComponent implements OnInit {

  _MessageService = inject(MessageService);
  _ActivatedRoute = inject(ActivatedRoute);
  username:WritableSignal<string> = signal('');
  Messages:WritableSignal<IMessage[]> = signal([]);
  messageContent:WritableSignal<string> = signal('');
  @ViewChild('messageForm') messageForm?:NgForm;
  ngOnInit(): void {
    this._ActivatedRoute.paramMap.subscribe({
      next:params => {
        const username = params.get('username');
        if(username){
          this.username.set(username)
          this.loadThreadMessage();
        } 
      }
    })
    
  }

  sendMessage(){
    this._MessageService.sendMessage(this.username(),this.messageContent()).subscribe({
      next:(res:IMessage)=>{
        this.Messages.update(prev => [...prev,res])
        this.messageForm?.reset();
      }
    })
  }

  loadThreadMessage():void{
    this._MessageService.getMessageThread(this.username()).subscribe({
      next:(res:IMessage[])=> this.Messages.set(res)
    })
  }
}
