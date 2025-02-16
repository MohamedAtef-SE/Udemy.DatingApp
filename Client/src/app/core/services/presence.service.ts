import { inject, Injectable, signal, WritableSignal } from '@angular/core';
import { Router } from '@angular/router';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { environment } from '../../../environments/environment';
import { IUser } from '../Models/Models';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {

  private _HubConnection?: HubConnection;
  private _ToastrService = inject(ToastrService);
  private _Router = inject(Router);

  OnlineUsers:WritableSignal<string[]> = signal([]);

  createHubConnection(user:IUser){
    this._HubConnection = new HubConnectionBuilder()
                              .withUrl(`${environment.hubsURL}/presence`,{
                                accessTokenFactory: ()=> user.token
                              })
                              .withAutomaticReconnect()
                              .build();
                              
    this._HubConnection.start().catch(error=> console.log(error));

    this._HubConnection.on("GetOnlineUsers",onlineUsersname => {
      this.OnlineUsers.set(onlineUsersname);
    })

    this._HubConnection.on('UserIsOnline',username => {
      this._ToastrService.info(`${username} is online.`)
      this.OnlineUsers.update((users)=>[...users,username])
    })

    this._HubConnection.on('UserIsOffline',username =>{
      this._ToastrService.warning(`${username} is offline.`)
      this.OnlineUsers.update(users => users.filter(user => user !== username))
      })

      this._HubConnection.on("NewMessageReceived",({username,knownAs})=>{
        this._ToastrService.info(`${knownAs} has sent you a new message! Click me to see it`)
                           .onTap
                           .pipe(take(1))
                           .subscribe(()=> this._Router.navigateByUrl(`/members/${username}/messages`) )
      })
  }

  stopHUbConnection(){
    if(this._HubConnection?.state === HubConnectionState.Connected){
      this._HubConnection.stop().catch(error => console.log(error));
    }
  }

}
