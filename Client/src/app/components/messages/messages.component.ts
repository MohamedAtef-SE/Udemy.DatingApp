import { AfterViewChecked, AfterViewInit, Component, computed, inject, OnInit, Signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TimeagoModule } from 'ngx-timeago';
import { IMessage, MessageParams } from '../../core/Models/Message';
import { MessageService } from '../../core/services/message.service';

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [TimeagoModule,RouterLink],
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.css'
})
export class MessagesComponent implements OnInit {
 
  _MessageService = inject(MessageService);
  messageParams = new MessageParams();
  Messages:Signal<IMessage[] | undefined> = computed(()=> this._MessageService.paginatedResult()?.items)

  ngOnInit() {
      this.loadMessages();
  }

  loadMessages(){
    this._MessageService.getMessage(this.messageParams);
  }

  getRoute(message:IMessage): string {
    if(this.messageParams.container == 'Outbox'){
      return `/members/${message.recipientUserName}`
    }
    else{
      return `/members/${message.senderUserName}`
    }
    }
    
    deleteMessage(id:number){
      this._MessageService.deleteMessage(id).subscribe({
        next:_=> this._MessageService.paginatedResult.update((prev)=>{
          if(prev && prev.items){
            prev.items.splice(prev.items.findIndex(m => m.id == id), 1);
          }
          return prev;
        })
      });
    }
  setContainer(container: string) {
    this.messageParams.container = container;
    this.loadMessages();
    } 
}
