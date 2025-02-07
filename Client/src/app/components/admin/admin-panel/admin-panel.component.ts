import { Component } from '@angular/core';
import { TabsComponent } from '../../shared/tabs/tabs.component';

@Component({
  selector: 'app-admin-panel',
  standalone: true,
  imports: [TabsComponent],
  templateUrl: './admin-panel.component.html',
  styleUrl: './admin-panel.component.css'
})
export class AdminPanelComponent {

}
