import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { environments } from '../../core/environments/environments';

@Component({
  selector: 'app-errors',
  standalone: true,
  imports: [],
  templateUrl: './errors.component.html',
  styleUrl: './errors.component.css'
})
export class ErrorsComponent {

  _HttpClient = inject(HttpClient);
  ValidationErrors:string[] = [];

  getBadRequest():void{
    this._HttpClient.get(`${environments.baseURL}/api/buggy/bad-request`).subscribe({
      next:(res)=>{
        console.log("success:")
        console.log(res);
      },
      error:(err)=>{
        console.log("error:")
        console.log(err)
      }
    })
  }

  getAuthError():void{
    this._HttpClient.get(`${environments.baseURL}/api/buggy/auth`).subscribe({
      next:(res)=>{
        console.log("success:")
        console.log(res);
      },
      error:(err)=>{
        console.log("error:")
        console.log(err)
      }
    })
  }

  getNotFound():void{
    this._HttpClient.get(`${environments.baseURL}/api/buggy/not-found`).subscribe({
      next:(res)=>{
        console.log("success:")
        console.log(res);
      },
      error:(err)=>{
        console.log("error:")
        console.log(err)
      }
    })
  }

  getValidationError():void{
    this._HttpClient.post(`${environments.baseURL}/api/buggy/validation`,{}).subscribe({
      next:(res)=>{
        console.log("success:")
        console.log(res);
      },
      error:(err)=>{
        console.log(err)
        this.ValidationErrors = err;
      }
    })
  }

  getServerException():void{
    this._HttpClient.get(`${environments.baseURL}/api/buggy/server-error`).subscribe({
      next:(res)=>{
        console.log("success:")
        console.log(res);
      },
      error:(err)=>{
        console.log("error:")
        console.log(err)
      }
    })
  }
}