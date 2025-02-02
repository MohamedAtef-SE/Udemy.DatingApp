import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { effect, inject, Injectable, signal, WritableSignal } from '@angular/core';
import { Observable, of, Subscription, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { IMember, IMemberUpdateForm } from '../Models/IMember';
import { IPhoto } from '../Models/IPhoto';
import { AccountService } from './account.service';
import { PaginatedResult } from '../Models/Pagination';
import { UserParams } from '../Models/userParams';


@Injectable({
  providedIn: 'root'
})

export class MembersService {

  constructor(private _HttpClient:HttpClient) {
    // don't invoke this method in service because its depend on
    // CurrentUser() who initiate on AppComponent and Services
    // in Angular built before components.
    //this.loadMember();
  }

 

  _AccountService = inject(AccountService);
  Member: WritableSignal<IMember> = signal({} as IMember);
  paginatedResult:WritableSignal<PaginatedResult<IMember[]> | null> = signal(null)
  pickedUserparams:WritableSignal<UserParams | null> = signal(null)
  memberCache = new Map();

  getAllMembers(userParams:UserParams): Subscription | void{
    
    const response = this.memberCache.get(Object.values(userParams).join('-'));

    if(response) return this.setPaginatedResponse(response);
 
    let params = this.setPaginationHeader(userParams);
    params = params.append('gender',userParams.gender);
    params = params.append('maxAge',userParams.maxAge);
    params = params.append('minAge',userParams.minAge);
    params = params.append('orderby',userParams.orderBy);

    if(userParams.city !== undefined)
       params = params.append('city',userParams.city)

    if(userParams.country !== undefined)
      params = params.append('city',userParams.country)

    return this._HttpClient.get<IMember[]>(`${environment.baseURL}/api/users`,{observe:'response',params})
    .subscribe({
      next: (response:HttpResponse<IMember[]>)=> {
       this.setPaginatedResponse(response);
       this.memberCache.set(Object.values(userParams).join('-'),response);
      }
      })
  }

  private setPaginatedResponse(response:HttpResponse<IMember[]>){
    this.paginatedResult.set({
      items: response.body as IMember[],
      paginationHeader: JSON.parse(response.headers.get('Pagination')!)
    })
  }
  private setPaginationHeader(userParams:UserParams) : HttpParams {
    let params = new HttpParams();
    let pageNumber = userParams.pageNumber;
    let pageSize = userParams.pageSize;

    if( pageNumber && pageSize){
      params = params.append("pageNumber",pageNumber);
      params = params.append("pageSize",pageSize);
    }
    return params;
  }

  getMember(username:string):Observable<any>{
    const member:IMember | undefined = this.findCachedMember(username)
    if(member) return of(member);

    return this._HttpClient.get<IMember>(`${environment.baseURL}/api/users/${username}`);
  }

  findCachedMember(username: string):IMember | undefined {
    const member:IMember | undefined = [...this.memberCache.values()]
    .reduce((cachedMembers,response)=> cachedMembers.concat(response.body),[])
    .find((m:IMember) => m.userName == username);
    return member;
  }
  
  updateMember(data:IMemberUpdateForm): Observable<any>{
    return this._HttpClient.put(`${environment.baseURL}/api/Users`,data).pipe(
      // tap(()=> {
      //   const username = this._AccountService.CurrentUser()?.username;
      //   if(username !== undefined){
      //     const member : IMember | undefined = this.Members().find(m => m.userName === username);
      //     if(member !== undefined){
      //       member.introduction = data.Introduction;
      //       member.lookingFor = data.LookingFor;
      //       member.interests = data.Interests;
      //       member.city = data.City;
      //       member.country = data.Country;
      //       this.Members.update( prevMembers =>
      //          prevMembers.map(m => m.userName == member.userName ? member : m)
      //          );
      //     }}}
      //   )
      )
  }

  setMemberPhoto(photo:IPhoto):Observable<any>{
    return this._HttpClient.put(`${environment.baseURL}/api/users/set-main-photo/${photo.id}`,{})
    .pipe(
      // tap(()=>{
      //   this.Members.update(members => members.map(m => {
         
      //     if(m.userName == this.Member().userName){
      //       console.log("found photo in this user")
      //       m.photoURL = photo.url
      //       m.photos.map(p => {
      //         if(p.url == photo.url){
      //           p.isMain = true;
      //         }
      //         else{
      //           p.isMain = false;
      //         }
      //         return p
      //       })
      //     }
      //     return m;
      //   }))

      //   this.Member.update(prev => {
      //     if(prev == null) return prev;
      //     const updatedMember : IMember = JSON.parse(JSON.stringify(prev));
      //     const photos = updatedMember.photos.map(p => {
      //       if(p.isMain == true) p.isMain = false;
      //       if(p.url == photo.url) p.isMain = true;
      //       return p;
      //     })

      //     return {
      //       ...updatedMember,
      //       photos: photos,
      //       photoURL: photo.url
      //     }
      //   })
      // })
    )
  }

  deleteMemberPhoto(photo:IPhoto):Observable<any>{
    return this._HttpClient.delete(`${environment.baseURL}/api/users/delete-photo/${photo.id}`)
    .pipe(
      // tap(()=>{

      //   this.Member.update(prev => {
      //     if(prev == null) return prev;
      //     const updatedMember : IMember = JSON.parse(JSON.stringify(prev));

      //     // set default photo if profile picture was deleted
      //     if(updatedMember.photoURL == photo.url){
      //       updatedMember.photoURL = "./assets/images/default.webp"
      //     }
      //     return {
      //       ...updatedMember,
      //       photos: updatedMember.photos.filter(p => p.id !== photo.id)
      //     }
      //   })

      //   if(this._AccountService.CurrentUser()?.photoURL == photo.url){
      //      // update-CurrentUser()
      //      this._AccountService.CurrentUser.update(prev =>{
      //       if(prev == null) return prev;
      //       return{
      //       ...prev,
      //       photoURL: "./assets/images/default.webp"
      //       }
      //     });
      //     const userJSON = JSON.stringify(this._AccountService.CurrentUser());
      //     localStorage.setItem("DateAppUserToken",userJSON)
      //   }

      //   // update all members list in case main profile pic deleted
      //   this.Members.update(members => members.map(m => {
      //     if(m.photoURL == photo.url && photo.isMain){
      //       m.photoURL = "../../../assets/images/default.webp"
      //     }

      //     return m;
      //   }))
      // })
    )
  }

  loadMember = ()=>{
    console.log("loading member...")
    const user = this._AccountService.CurrentUser();
    if(!user) return;
    this.getMember(user.username).subscribe({
      next:(res:IMember)=>{
        this.Member.set(res);
        console.log("User loaded...")
      }
    })
  }
}