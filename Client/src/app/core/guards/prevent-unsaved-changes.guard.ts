import { CanDeactivateFn } from '@angular/router';
import { EditProfileComponent } from '../../components/members/member-edit/edit-profile/edit-profile.component';

export const preventUnsavedChangesGuard: CanDeactivateFn<EditProfileComponent> = (component, currentRoute, currentState, nextState) => {
 if(component.editForm?.dirty){
  console.log('--------------------')
  console.log("current Route:")
  console.log(currentRoute)
  console.log('--------------------')
  console.log("current State:")
  console.log(currentState)
  console.log('--------------------')
  console.log("next State:")
  console.log(nextState)
  console.log('--------------------')
  return confirm("Are you sure you want to continue? Any unsaved changes will be lost");
 }

 return true;
};
