import { NgModule } from '@angular/core'
import { RouterModule, Routes } from '@angular/router'
import { AuthGuard } from './guards/auth.guard'
import { PreventUnsavedChangesGuard } from './guards/prevent-unsaved-changes.guard'
import { MessagesComponent } from './components/messages/messages.component'
import { AdminPanelComponent } from './components/admin/admin-panel/admin-panel.component'
import { ListsComponent } from './components/lists/lists.component'
import { MemberDetailComponent } from './components/members/member-detail/member-detail.component'
import { MemberListComponent } from './components/members/member-list/member-list.component'
import { MemberEditComponent } from './components/members/member-edit/member-edit.component'
import { HomeComponent } from './components/home/home.component'
import { NotFoundComponent } from './components/errors/not-found/not-found.component'
import { ServerErrorComponent } from './components/errors/server-error/server-error.component'
import { MemberDetailedResolver } from './resolvers/member-detailed.resolver'
import { AdminGuard } from './guards/admin.guard'

const routes: Routes = [
	{ path: '', component: HomeComponent },
	{
		path: '',
		runGuardsAndResolvers: 'always',
		canActivate: [AuthGuard],
		children: [
			{ path: 'members', component: MemberListComponent },
			{ path: 'members/:username', component: MemberDetailComponent, resolve: { member: MemberDetailedResolver } },
			{ path: 'member/edit', component: MemberEditComponent, canDeactivate: [PreventUnsavedChangesGuard] },
			{ path: 'lists', component: ListsComponent },
			{ path: 'messages', component: MessagesComponent },
			{ path: 'admin', component: AdminPanelComponent, canActivate: [AdminGuard] },
		]
	},
	{ path: 'not-found', component: NotFoundComponent },
	{ path: 'server-error', component: ServerErrorComponent },
	{ path: '**', component: NotFoundComponent, pathMatch: 'full' }
]

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule]
})

export class AppRoutingModule { }
