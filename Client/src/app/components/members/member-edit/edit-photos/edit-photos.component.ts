import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { IPhoto } from '../../../../core/Interfaces/IPhoto';

@Component({
  selector: 'app-edit-photos',
  standalone: true,
  imports: [],
  templateUrl: './edit-photos.component.html',
  styleUrl: './edit-photos.component.css'
})
export class EditPhotosComponent implements OnInit {

  Photos:WritableSignal<IPhoto[]> = signal([]);

  ngOnInit(): void {
      this.Photos.set(history.state.data.photos)
  }

}
