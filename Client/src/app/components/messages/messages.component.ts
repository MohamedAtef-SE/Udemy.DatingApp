import { AfterViewChecked, AfterViewInit, Component, computed, inject, OnInit, Signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TimeagoModule } from 'ngx-timeago';
import { IMessage, MessageParams } from '../../core/Models/Message';
import { MessageService } from '../../core/services/message.service';
import { AnimationOptions, LottieComponent } from 'ngx-lottie';
import { AnimationItem } from 'lottie-web';

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [TimeagoModule,RouterLink,LottieComponent],
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
      return `/members/${message.recipientUserName}/messages`
    }
    else{
      return `/members/${message.senderUserName}/messages`
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
    
    options: AnimationOptions = {
      path: '/assets/svg/desert.json',
    };
    
    animationCreated(animationItem: AnimationItem): void {
    console.log(animationItem);
  }
}
