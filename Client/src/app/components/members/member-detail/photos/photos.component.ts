import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { IPhoto } from '../../../../core/Models/IPhoto';

@Component({
  selector: 'app-photos',
  standalone: true,
  imports: [GalleryModule],
  templateUrl: './photos.component.html',
  styleUrl: './photos.component.css'
})
export class PhotosComponent implements OnInit  {

  Photos:WritableSignal<IPhoto[]> = signal([])
  HideModel : WritableSignal<boolean> = signal(true);
  Images: GalleryItem[] = [];

  ngOnInit():void{
    //this.Photos.set(history.state.data)
    history.state.data.map((p:IPhoto) => {
      this.Images.push(new ImageItem({src: p.url,thumb: p.url}))
    })
  }

  toggleModel = ()=> {
    this.HideModel.update(prev => !prev);
    }
}
