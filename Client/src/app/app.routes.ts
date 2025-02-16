import { Routes } from '@angular/router';
import { ErrorsComponent } from './components/errors/errors.component';
import { ServerErrorComponent } from './components/errors/server-error/server-error.component';
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
import { AdminPanelComponent } from './components/admin/admin-panel/admin-panel.component';
import { adminGuard } from './core/guards/admin.guard';
import { RegisterComponent } from './components/register/register.component';
import { unauthGuard } from './core/guards/unauth.guard';




export const routes: Routes = [
    {
        path: '',
        redirectTo: '',
        pathMatch:'full'
    },
    {
        path: '',
        loadComponent:()=> import('./components/home/home.component').then(c => c.HomeComponent)
        ,canActivate:[unauthGuard]},
    {
        path:'register',
        loadComponent:()=> import('./components/register/register.component').then(c => c.RegisterComponent)
    },
    {path:'', canActivate: [authGuard],runGuardsAndResolvers:'always',children:[
        {
            path:'members',
            loadComponent:()=> import('./components/members/member-list/member-list.component').then(c => c.MemberListComponent)
        },
        {
            path: 'members/:memberName',
            loadComponent:()=> import('./components/members/member-detail/member-detail.component').then(c => c.MemberDetailComponent),
            resolve:{member: memberDetailedResolver},
            children:[
            {
                path:'about',
                loadComponent:()=> import('./components/members/member-detail/about/about.component').then(c => c.AboutComponent),
            },
            {
                path:'interests',
                loadComponent:()=> import('./components/members/member-detail/interests/interests.component').then(c => c.InterestsComponent),
            },
            {
                path: 'photos',
                loadComponent:()=> import('./components/members/member-detail/photos/photos.component').then(c => c.PhotosComponent),
            },
            {
                path: 'messages',
                loadComponent:()=> import('./components/members/member-detail/member-messages/member-messages.component').then(c => c.MemberMessagesComponent),
            }
        ]
    },
        {
            path:'member/edit',
            loadComponent:()=> import('./components/members/member-edit/member-edit.component').then(c => c.MemberEditComponent),
            children:[
            {
                path:'profile',
                loadComponent:()=> import('./components/members/member-edit/edit-profile/edit-profile.component').then(c => c.EditProfileComponent),
                canDeactivate:[preventUnsavedChangesGuard]
            },
            {
                path: 'photos',
                loadComponent:()=> import('./components/members/member-edit/edit-photos/edit-photos.component').then(c => c.EditPhotosComponent),
            }
        ]
    },
        {
            path: 'lists',
            loadComponent:()=> import('./components/lists/lists.component').then(c => c.ListsComponent),
        },
        {
            path: 'messages', 
            loadComponent:()=> import('./components/messages/messages.component').then(c => c.MessagesComponent),
        },
        {
            path:'admin',
            loadComponent:()=> import('./components/admin/admin-panel/admin-panel.component').then(c => c.AdminPanelComponent),
            canActivate:[adminGuard]
        }
    ]
},
    {
        path: 'errors',
        loadComponent:()=> import('./components/errors/errors.component').then(c => c.ErrorsComponent),
        canActivate: [adminGuard]},
    {
        path: 'not-found',
        loadComponent:()=> import('./components/not-found/not-found.component').then(c => c.NotFoundComponent),
    },
    {
        path:'server-error',
        loadComponent:()=> import('./components/errors/server-error/server-error.component').then(c => c.ServerErrorComponent),
    },
    {
        path: '**',
        loadComponent:()=> import('./components/not-found/not-found.component').then(c => c.NotFoundComponent),
    }
];