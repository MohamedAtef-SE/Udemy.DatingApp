import { Routes } from '@angular/router';
import { ErrorsComponent } from './components/errors/errors.component';
import { HomeComponent } from './components/home/home.component';
import { ListsComponent } from './components/lists/lists.component';
import { MemberDetailComponent } from './components/members/member-detail/member-detail.component';
import { MemberListComponent } from './components/members/member-list/member-list.component';
import { MessagesComponent } from './components/messages/messages.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { authGuard } from './core/gurards/auth-guard';
import { ServerErrorComponent } from './components/errors/server-error/server-error.component';

export const routes: Routes = [
    {path: '',redirectTo: '',pathMatch:'full'},
    {path: '',component: HomeComponent},
    {path:'', canActivate: [authGuard],runGuardsAndResolvers:'always',children:[
        {path:'members',component: MemberListComponent},
        {path: 'members/:id',component: MemberDetailComponent},
        {path: 'lists',component: ListsComponent},
        {path: 'messages', component: MessagesComponent},
    ]},
    {path: 'errors',component: ErrorsComponent},
    {path: 'not-found',component: NotFoundComponent},
    {path:'server-error',component: ServerErrorComponent},
    {path: '**',component: NotFoundComponent}
];