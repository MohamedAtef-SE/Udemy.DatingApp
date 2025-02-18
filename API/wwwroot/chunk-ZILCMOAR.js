import{a as g,f as U}from"./chunk-I5PP4UYU.js";import{a as n,b as m,c as M}from"./chunk-L3R4MWKK.js";import{G as u,J as h,Jb as f,M as b,N as p,k as l,va as d}from"./chunk-R4PBEABJ.js";import{a,b as c}from"./chunk-5WK33CEB.js";var j=(()=>{class s{constructor(e){this._HttpClient=e,this._AccountService=p(U),this._PaginatationService=p(g),this.Member=d({}),this.pickedUserparams=d(null),this.memberCache=new Map,this.loadMember=()=>{console.log("loading member...");let i=this._AccountService.CurrentUser();i&&this.getMember(i.username).subscribe({next:t=>{this.Member.set(t),console.log("User loaded...")}})}}getAllMembers(e){let i=this.memberCache.get(Object.values(e).join("-"));if(i)return m(i,this._PaginatationService.paginatedResult);let t=M(e);return t=t.append("gender",e.gender),t=t.append("maxAge",e.maxAge),t=t.append("minAge",e.minAge),t=t.append("orderby",e.orderBy),e.city!==void 0&&(t=t.append("city",e.city)),e.country!==void 0&&(t=t.append("city",e.country)),this._HttpClient.get(`${n.baseURL}/users`,{observe:"response",params:t}).subscribe({next:r=>{m(r,this._PaginatationService.paginatedResult),this.memberCache.set(Object.values(e).join("-"),r)}})}setPaginationHeader(e){throw new Error("Method not implemented.")}getMember(e){let i=this.findCachedMember(e);return i?l(i):this._HttpClient.get(`${n.baseURL}/users/${e}`)}findCachedMember(e){return[...this.memberCache.values()].reduce((t,r)=>t.concat(r.body),[]).find(t=>t.userName==e)}updateMember(e){return this._HttpClient.put(`${n.baseURL}/Users`,e).pipe()}setMemberPhoto(e){return this._HttpClient.put(`${n.baseURL}/users/set-main-photo/${e.id}`,{}).pipe(u(()=>{this.Member.update(i=>{if(i==null)return i;let t=JSON.parse(JSON.stringify(i)),r=t.photos.map(o=>(o.isMain==!0&&(o.isMain=!1),o.url==e.url&&(o.isMain=!0),o));return c(a({},t),{photos:r,photoURL:e.url})})}))}deleteMemberPhoto(e){return this._HttpClient.delete(`${n.baseURL}/users/delete-photo/${e.id}`).pipe(u(()=>{if(this.Member.update(i=>{if(i==null)return i;let t=JSON.parse(JSON.stringify(i));return t.photoURL==e.url&&(t.photoURL="./assets/images/default.webp"),c(a({},t),{photos:t.photos.filter(r=>r.id!==e.id)})}),this._AccountService.CurrentUser()?.photoURL==e.url){this._AccountService.CurrentUser.update(t=>t==null?t:c(a({},t),{photoURL:"./assets/images/default.webp"}));let i=JSON.stringify(this._AccountService.CurrentUser());localStorage.setItem("DateAppUser",i)}}))}static{this.\u0275fac=function(i){return new(i||s)(b(f))}}static{this.\u0275prov=h({token:s,factory:s.\u0275fac,providedIn:"root"})}}return s})();export{j as a};
