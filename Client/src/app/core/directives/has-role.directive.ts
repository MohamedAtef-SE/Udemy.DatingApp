import { computed, Directive, inject, Input, OnInit, Signal, TemplateRef, ViewContainerRef } from '@angular/core';
import { AccountService } from '../services/account.service';

@Directive({
  selector: '[appHasRole]',
  standalone: true
})
export class HasRoleDirective implements OnInit {

  @Input() appHasRole:string[] = [];
  _AccountService = inject(AccountService);
  _ViewContainerRef = inject(ViewContainerRef);
  _TemplateRef = inject(TemplateRef);
  UserRoles:Signal<string[]> = computed(()=>this._AccountService.UserRoles());

  ngOnInit(): void {
    const hasRole = this.UserRoles().some((userRole:string)=> this.appHasRole.includes(userRole));
    if(hasRole){
      this._ViewContainerRef.createEmbeddedView(this._TemplateRef)
    }else{
      this._ViewContainerRef.clear();
    }
  }

}
