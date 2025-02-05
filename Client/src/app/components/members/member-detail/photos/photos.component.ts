import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { IPhoto } from '../../../../core/Models/IPhoto';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-photos',
  standalone: true,
  imports: [GalleryModule],
  templateUrl: './photos.component.html',
  styleUrl: './photos.component.css'
})
export class PhotosComponent implements OnInit  {

  _ActivatedRoute = inject(ActivatedRoute);
  Images: GalleryItem[] = [];

  ngOnInit():void{
    // comes from queryParams from member-detail.component.html
    this._ActivatedRoute.queryParams.subscribe(param => {
      let images:GalleryItem[] = JSON.parse(param['data'])
      this.Images = images;
    })
  }
}
