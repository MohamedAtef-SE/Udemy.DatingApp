import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { UsersService } from '../../core/services/users.service';
import { Subscription } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent implements OnInit , OnDestroy{
  
  private readonly _UsersService = inject(UsersService);
  UsersSubscribe!:Subscription
  users:any

 
  ngOnInit(): void {
    this.UsersSubscribe = this._UsersService.getAllUsers().subscribe({
      next:(res)=>{
        console.log(res)
      },
      error: (err:HttpErrorResponse)=>{
        console.log(err.message);
      },
      complete:()=>{
        console.log("Completed....");
      }
    })
  }


  ngOnDestroy(): void {
    throw new Error('Method not implemented.');
  }

}
