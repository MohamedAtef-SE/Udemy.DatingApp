import { Component } from '@angular/core';
import { UserManagementComponent } from "../../admin/user-management/user-management.component";
import { PhotoManagementComponent } from "../../admin/photo-management/photo-management.component";
import { HasRoleDirective } from '../../../core/directives/has-role.directive';

@Component({
  selector: 'app-tabs',
  standalone: true,
  imports: [UserManagementComponent, PhotoManagementComponent,HasRoleDirective],
  templateUrl: './tabs.component.html',
  styleUrl: './tabs.component.css'
})
export class TabsComponent {
  activeTab: string = 'user-management'; // Default active tab

  setActiveTab(tabName: string) {
    this.activeTab = tabName;
  }
}
