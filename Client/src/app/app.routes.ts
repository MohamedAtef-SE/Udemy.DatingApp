import { Routes } from '@angular/router';
import { ErrorsComponent } from './components/errors/errors.component';
import { ServerErrorComponent } from './components/errors/server-error/server-error.component';
import { HomeComponent } from './components/home/home.component';
import { ListsComponent } from './components/lists/lists.component';
import { AboutComponent } from './components/members/member-detail/about/about.component';
import { InterestsComponent } from './components/members/member-detail/interests/interests.component';
import { MemberDetailComponent } from './components/members/member-detail/member-detail.component';
import { MemberMessagesComponent } from './components/members/member-detail/member-messages/member-messages.component';
import { PhotosComponent } from './components/members/member-detail/photos/photos.component';
import { EditPhotosComponent } from './components/members/member-edit/edit-photos/edit-photos.component';
import { EditProfileComponent } from './components/members/member-edit/edit-profile/edit-profile.component';
import { MemberEditComponent } from './components/members/member-edit/member-edit.component';
import { MemberListComponent } from './components/members/member-list/member-list.component';
import { MessagesComponent } from './components/messages/messages.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { authGuard } from './core/guards/auth-guard';
import { preventUnsavedChangesGuard } from './core/guards/prevent-unsaved-changes.guard';
import { memberDetailedResolver } from './core/resolvers/member-detailed.resolver';

export const routes: Routes = [
    {path: '',redirectTo: '',pathMatch:'full'},
    {path: '',component: HomeComponent},
    {path:'', canActivate: [authGuard],runGuardsAndResolvers:'always',children:[
        {path:'members',component: MemberListComponent},
        {path: 'members/:username',component: MemberDetailComponent,resolve:{member: memberDetailedResolver},children:[
            {path:'about',component: AboutComponent},
            {path:'interests',component: InterestsComponent},
            {path: 'photos',component: PhotosComponent},
            {path: 'messages',component: MemberMessagesComponent}
        ]},
        {path:'member/edit',component: MemberEditComponent,children:[
            {path:'profile',component: EditProfileComponent,canDeactivate:[preventUnsavedChangesGuard]},
            {path: 'photos', component: EditPhotosComponent}
        ]},
        {path: 'lists',component: ListsComponent},
        {path: 'messages', component: MessagesComponent},
    ]},
    {path: 'errors',component: ErrorsComponent},
    {path: 'not-found',component: NotFoundComponent},
    {path:'server-error',component: ServerErrorComponent},
    {path: '**',component: NotFoundComponent}
];