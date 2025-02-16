import { HttpClient } from '@angular/common/http';
import { Injectable, signal, WritableSignal } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { Observable, Subscription } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Group } from '../Models/Group';
import { IMessage, MessageParams } from '../Models/Message';
import { IUser } from '../Models/Models';
import { PaginatedResult } from '../Models/Pagination';
import { setPaginatedResponse, setPaginationHeader } from './paginationHelper';

@Injectable({
 providedIn: 'root'
})
export class MessageService {

  constructor(private _HttpClient:HttpClient) { }

  paginatedResult:WritableSignal<PaginatedResult<IMessage> | null> = signal(null);
  _HubConnection?:HubConnection;

  MessageThread:WritableSignal<IMessage[]> = signal([]);

  
createHubConnection(user:IUser,otherUsername:string){
  this._HubConnection = new HubConnectionBuilder()
                            .withUrl(`${environment.hubsURL}/message?user=${otherUsername}`,{
                              accessTokenFactory: ()=> user.token
                            })
                            .withAutomaticReconnect()
                            .build();

  this._HubConnection.start().catch(error => console.log(error));

  // create event listener
  this._HubConnection.on("ReceiveMessageThread",messages => {
    this.MessageThread.set(messages);
  })

  this._HubConnection.on("NewMessage",newMessage => {
    this.MessageThread.update(prevMessages => [...prevMessages,newMessage])
  })

  this._HubConnection.on("UpdatedGroup",(group:Group)=>{
    if(group.connections.some(c => c.username === otherUsername)){
      this.MessageThread.update(messages => {
        messages.forEach(message => {
          if(!message.dateRead){
            message.dateRead = new Date(Date.now());
          }
        })
        return messages;
      })
    }
  })
                         
}

stopHubConnection(){
  if(this._HubConnection?.state === HubConnectionState.Connected){
    this._HubConnection.stop().catch(error => console.log(error))
    console.log("connection stopped successfully.")
  }
}

  getMessage(messageParams:MessageParams):Subscription{
    let params = setPaginationHeader(messageParams);

    if(messageParams.container){
      params = params.append("Container",messageParams.container)
    }

    return this._HttpClient.get<IMessage[]>(`${environment.baseURL}/messages`,{observe:'response',params})
    .subscribe({
    next:response => setPaginatedResponse(response,this.paginatedResult)
  })
  }

// getMessageThread(username:string):Observable<any>{
//   return this._HttpClient.get<IMessage[]>(`${environment.baseURL}/messages/thread/${username}`)
// }

async sendMessage(recipientUserName:string,content:string):Promise<any>{
  console.log(this._HubConnection?.state) // connected
  // Invoke from MessageHub in server-side 'SendMessage' Method.
  return this._HubConnection?.invoke('SendMessage',{recipientUserName,content})
}

deleteMessage(id:number):Observable<any>{
return this._HttpClient.delete(`${environment.baseURL}/messages/${id}`)
}
}
