import { Component, Input, input, InputSignal } from "@angular/core";



@Component({
    selector: 'app-photo-modal',
    standalone: true,
    imports:[],
    templateUrl: './modal.component.html',
    styleUrl: './modal.component.css'
})

export class ModalPhotoComponent{
    HiddenTheModal:InputSignal<boolean> = input(false);
    ImgSrc: InputSignal<string> = input.required();
    @Input() toggle! :()=>void;
}