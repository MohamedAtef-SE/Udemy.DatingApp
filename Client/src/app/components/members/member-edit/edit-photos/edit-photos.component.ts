import { DecimalPipe, NgClass, NgFor, NgIf, NgStyle } from '@angular/common';
import { Component, computed, inject, OnInit, Signal } from '@angular/core';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { environment } from '../../../../../environments/environment';
import { IMember } from '../../../../core/Models/IMember';
import { IPhoto } from '../../../../core/Models/IPhoto';
import { AccountService } from '../../../../core/services/account.service';
import { MembersService } from '../../../../core/services/members.service';




@Component({
  selector: 'app-edit-photos',
  standalone: true,
  imports: [NgIf,NgFor,NgStyle,NgClass,FileUploadModule,DecimalPipe],
templateUrl: './edit-photos.component.html',
  styleUrl: './edit-photos.component.css'
})
export class EditPhotosComponent implements OnInit {

  private readonly _AccountService = inject(AccountService);
  private readonly _MemberService = inject(MembersService);
  private readonly _ToastService = inject(ToastrService);
  Member:Signal<IMember> = computed(()=> this._MemberService.Member())
  hasBaseDropZoneOver: boolean = false;
  uploader: FileUploader|undefined;
  baseURL = environment.baseURL;

  ngOnInit(): void {
      this.initializeUploader();
  }

  changeProfilePicture(photo:IPhoto):void{
    if(photo.url == this._AccountService.CurrentUser()?.photoURL) return;

    this._MemberService.setMemberPhoto(photo)
    .subscribe({
      next:_=>{
        // update current-logged-in-user with new picture url
        const currentUser = this._AccountService.CurrentUser();
        if(currentUser){
          currentUser.photoURL = photo.url
          this._AccountService.CurrentUser.set(currentUser);
          const userAsJSON = JSON.stringify(this._AccountService.CurrentUser());
          localStorage.setItem("HommiesUser",userAsJSON)
        }
      }
    })
  }

  deletePhoto(photo:IPhoto):void{
    this._MemberService.deleteMemberPhoto(photo).subscribe({
      next:()=>{
        this._ToastService.success("photo deleted successfully","Our Date")
      }
    })
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
    }

    initializeUploader(){
      this.uploader = new FileUploader({
        url: `${this.baseURL}/users/add-photo`,
        authToken: `Bearer ${this._AccountService.CurrentUser()?.token}`,
        isHTML5: true,
        allowedFileType: ['image'],
        removeAfterUpload: true,
        autoUpload: false,
        maxFileSize: 10 * 1024 * 1024
      });

      this.uploader.onAfterAddingFile = (file)=>{
        file.withCredentials = false;
      }

      this.uploader.onSuccessItem = (item,response,status,headers)=>{
        const photo = JSON.parse(response);
        this.Member().photos.push(photo);
      }


    }
}
