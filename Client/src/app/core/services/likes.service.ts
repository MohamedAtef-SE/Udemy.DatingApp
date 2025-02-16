import { HttpClient, HttpResponse } from '@angular/common/http';
import { inject, Injectable, signal, WritableSignal } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { environment } from '../../../environments/environment';
import { IMember } from '../Models/IMember';
import { LikesParams } from './../Models/likesParams';
import { PaginationService } from './pagination.service';
import { setPaginatedResponse, setPaginationHeader } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class LikesService {

  constructor(private _HttpClient:HttpClient) {}

  _PaginationService = inject(PaginationService);
  likeIds:WritableSignal<number[]> = signal([]);

  toggleLike(targetId:number):Observable<any>{
    return this._HttpClient.post(`${environment.baseURL}/likes/${targetId}`,{})
  }

  getLikes(likesParams:LikesParams){

    let params = setPaginationHeader(likesParams);
    params = params.append('predicate',likesParams.predicate)
    
    return this._HttpClient.get<IMember[]>(`${environment.baseURL}/likes`,{observe:'response',params}).subscribe({
      next:(res:HttpResponse<IMember[]>)=>{
        
        setPaginatedResponse<IMember>(res,this._PaginationService.paginatedResult)
      } 
        
    })
  }

  getLikeIds():Subscription{
    return this._HttpClient.get<number[]>(`${environment.baseURL}/likes/list`).subscribe({
      next:(res:number[])=>{
        this.likeIds.set(res);
      }
    })
  }
}
