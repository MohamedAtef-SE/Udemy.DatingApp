import{a as W,d as T,e as k,g as L,h as N,i as B,s as I}from"./chunk-T4MTWN4K.js";import{a as V}from"./chunk-ZILCMOAR.js";import{c as P}from"./chunk-I5PP4UYU.js";import"./chunk-L3R4MWKK.js";import"./chunk-XAZLOLJU.js";import{Da as y,Ha as n,Ia as r,Ja as x,N as _,Na as b,P as M,Pa as C,Qa as a,Tb as F,W as m,Wa as h,X as s,Xa as w,Ya as S,_a as l,cb as u,db as p,eb as g,gb as E,la as c,sb as v,xa as f}from"./chunk-R4PBEABJ.js";import"./chunk-5WK33CEB.js";var Q=["editForm"];function R(d,A){if(d&1){let t=b();n(0,"section",1)(1,"h2",2),l(2," Attention "),n(3,"span",3),x(4,"i",4),l(5," Any unsaved information will be lost!! "),r()(),n(6,"form",5,0),C("ngSubmit",function(){m(t);let e=a();return s(e.updateSubmit())}),n(8,"div",6)(9,"h4",7),l(10,"Introduction"),r(),n(11,"textarea",8),g("ngModelChange",function(e){m(t);let i=a();return p(i.Member().introduction,e)||(i.Member().introduction=e),s(e)}),r()(),n(12,"div",6)(13,"h4",7),l(14,"Looking For"),r(),n(15,"textarea",9),g("ngModelChange",function(e){m(t);let i=a();return p(i.Member().lookingFor,e)||(i.Member().lookingFor=e),s(e)}),r()(),n(16,"div",6)(17,"h4",7),l(18,"Interests"),r(),n(19,"textarea",10),g("ngModelChange",function(e){m(t);let i=a();return p(i.Member().interests,e)||(i.Member().interests=e),s(e)}),r()(),n(20,"div",11)(21,"h4",7),l(22,"Location"),r(),n(23,"div",12)(24,"div",13)(25,"label",14),l(26,"City"),r(),n(27,"input",15),g("ngModelChange",function(e){m(t);let i=a();return p(i.Member().city,e)||(i.Member().city=e),s(e)}),r()(),n(28,"div",13)(29,"label",16),l(30,"Country"),r(),n(31,"input",17),g("ngModelChange",function(e){m(t);let i=a();return p(i.Member().country,e)||(i.Member().country=e),s(e)}),r()()()()()()}if(d&2){let t=a();c(11),u("ngModel",t.Member().introduction),c(4),u("ngModel",t.Member().lookingFor),c(4),u("ngModel",t.Member().interests),c(8),u("ngModel",t.Member().city),c(4),u("ngModel",t.Member().country)}}var U=(()=>{class d{constructor(){this.Member=v(()=>this._MembersService.Member()),this._MembersService=_(V),this._ToastrService=_(P),this._Router=_(F)}updateSubmit(){let t={City:this.Member().city,Country:this.Member().country,Interests:this.Member().interests,Introduction:this.Member().introduction,LookingFor:this.Member().lookingFor};this._MembersService.updateMember(t).subscribe({next:o=>{this._ToastrService.success("Profile updated successfully"),this._Router.navigate([`members/${this.Member().userName}`])}}),this.editForm?.reset(this.Member())}static{this.\u0275fac=function(o){return new(o||d)}}static{this.\u0275cmp=M({type:d,selectors:[["app-edit-profile"]],viewQuery:function(o,e){if(o&1&&h(Q,5),o&2){let i;w(i=S())&&(e.editForm=i.first)}},standalone:!0,features:[E],decls:1,vars:1,consts:[["editForm","ngForm"],[1,"p-10"],[1,"text-white","rounded","px-4","py-2","bg-yellow-300","font-semibold"],[1,"font-normal"],[1,"fa-solid","fa-circle-exclamation"],["id","EditForm",3,"ngSubmit"],[1,"my-2"],[1,"font-semibold","mb-2"],["name","introduction","rows","4",1,"rounded","py-2","px-4","w-full",3,"ngModelChange","ngModel"],["name","lookingFor","rows","4",1,"rounded","py-2","px-4","w-full",3,"ngModelChange","ngModel"],["name","interests","rows","4",1,"rounded","py-2","px-4","w-full",3,"ngModelChange","ngModel"],[1,"border","p-3","rounded"],[1,"flex"],[1,"flex","justify-center","items-center"],["for","city",1,"p-2","block","text-sm","font-semibold"],["type","text","name","city",1,"rounded","py-2","px-4",3,"ngModelChange","ngModel"],["for","country",1,"p-2","block","text-sm","font-semibold"],["type","text","name","country",1,"rounded","py-2","px-4",3,"ngModelChange","ngModel"]],template:function(o,e){o&1&&f(0,R,32,5,"section",1),o&2&&y(0,e.Member()?0:-1)},dependencies:[I,B,W,T,k,N,L]})}}return d})();export{U as EditProfileComponent};
