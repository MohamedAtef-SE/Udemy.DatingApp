import { HttpClient } from '@angular/common/http';
import { Injectable, signal, WritableSignal } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { environment } from '../../../environments/environment';
import { IMessage, MessageParams } from '../Models/Message';
import { PaginatedResult } from '../Models/Pagination';
import { setPaginatedResponse, setPaginationHeader } from './paginationHelper';

@Injectable({
 providedIn: 'root'
})
export class MessageService {

  constructor(private _HttpClient:HttpClient) { }

  paginatedResult:WritableSignal<PaginatedResult<IMessage> | null> = signal(null);

  getMessage(messageParams:MessageParams):Subscription{
    let params = setPaginationHeader(messageParams);

    if(messageParams.container){
      params = params.append("Container",messageParams.container)
    }

    return this._HttpClient.get<IMessage[]>(`${environment.baseURL}/api/messages`,{observe:'response',params})
    .subscribe({
    next:response => setPaginatedResponse(response,this.paginatedResult)
  })
  }
getMessageThread(username:string):Observable<any>{
  return this._HttpClient.get<IMessage[]>(`${environment.baseURL}/api/messages/thread/${username}`)
}

sendMessage(username:string,content:string):Observable<any>{
return this._HttpClient.post<IMessage>(`${environment.baseURL}/api/messages`,{recipientUserName: username, content})
}

deleteMessage(id:number):Observable<any>{
return this._HttpClient.delete(`${environment.baseURL}/api/messages/${id}`)
}
}
