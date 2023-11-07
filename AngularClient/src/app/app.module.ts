import { NgModule } from '@angular/core'
import { BrowserModule } from '@angular/platform-browser'
import { AppRoutingModule } from './app-routing.module'
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'

import { SharedModule } from './modules/shared.module'
import { AppComponent } from './app.component'
import { NavComponent } from './components/nav/nav.component'
import { HomeComponent } from './components/home/home.component'
import { RegisterComponent } from './components/register/register.component'
import { MemberListComponent } from './components/members/member-list/member-list.component'
import { MemberDetailComponent } from './components/members/member-detail/member-detail.component'
import { ListsComponent } from './components/lists/lists.component'
import { MessagesComponent } from './components/messages/messages.component'
import { NotFoundComponent } from './components/errors/not-found/not-found.component'
import { ServerErrorComponent } from './components/errors/server-error/server-error.component'
import { MemberCardComponent } from './components/members/member-card/member-card.component'
import { MemberEditComponent } from './components/members/member-edit/member-edit.component'
import { ErrorInterceptor } from './interceptors/error.interceptor'
import { JwtInterceptor } from './interceptors/jwt.interceptor'
import { LoadingInterceptor } from './interceptors/loading.interceptor';
import { PhotoEditorComponent } from './components/members/photo-editor/photo-editor.component';
import { TextInputComponent } from './components/forms/text-input/text-input.component';
import { DateInputComponent } from './components/forms/date-input/date-input.component';
import { MemberMessagesComponent } from './components/members/member-messages/member-messages.component';
import { AdminPanelComponent } from './components/admin/admin-panel/admin-panel.component';
import { HasRoleDirective } from './directives/has-role.directive';
import { UserManagementComponent } from './components/admin/user-management/user-management.component';
import { PhotoManagementComponent } from './components/admin/photo-management/photo-management.component';
import { RolesModalComponent } from './components/modals/roles-modal/roles-modal.component'

@NgModule({
	declarations: [
		AppComponent,
  		NavComponent,
		HomeComponent,
		RegisterComponent,
		MemberListComponent,
		MemberDetailComponent,
		ListsComponent,
		MessagesComponent,
		NotFoundComponent,
		ServerErrorComponent,
		MemberCardComponent,
  		MemberEditComponent,
    	PhotoEditorComponent,
		TextInputComponent,
		DateInputComponent,
		MemberMessagesComponent,
		AdminPanelComponent,
		HasRoleDirective,
		UserManagementComponent,
		PhotoManagementComponent,
		RolesModalComponent
	],
	imports: [
		BrowserModule,
		AppRoutingModule,
		HttpClientModule,
  		BrowserAnimationsModule,
		FormsModule,
		ReactiveFormsModule,
		SharedModule
	],
	providers: [
		{ provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
		{ provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
		{ provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true }
	],
	bootstrap: [AppComponent]
})
export class AppModule { }
