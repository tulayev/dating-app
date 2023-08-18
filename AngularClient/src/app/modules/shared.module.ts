import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { ToastrModule } from 'ngx-toastr'
import { BsDropdownModule } from 'ngx-bootstrap/dropdown'
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker'
import { ButtonsModule } from 'ngx-bootstrap/buttons'
import { PaginationModule } from 'ngx-bootstrap/pagination'
import { TabsModule } from 'ngx-bootstrap/tabs'
import { ModalModule } from 'ngx-bootstrap/modal'
import { TimeagoModule } from 'ngx-timeago'
import { NgxGalleryModule } from '@kolkov/ngx-gallery'
import { NgxSpinnerModule } from 'ngx-spinner'
import { FileUploadModule } from 'ng2-file-upload'

@NgModule({
	declarations: [],
	imports: [
		CommonModule,
		BsDropdownModule.forRoot(),
		BsDatepickerModule.forRoot(),
		ButtonsModule.forRoot(),
		PaginationModule.forRoot(),
		ToastrModule.forRoot({
			positionClass: 'toast-bottom-right'
		}),
		TabsModule.forRoot(),
		ModalModule.forRoot(),
		TimeagoModule.forRoot(),
		NgxGalleryModule,
		NgxSpinnerModule,
		FileUploadModule
	],
	exports: [
		BsDropdownModule,
		BsDatepickerModule,
		ButtonsModule,
		PaginationModule,
		ToastrModule,
		TabsModule,
		ModalModule,
		TimeagoModule,
		NgxGalleryModule,
		NgxSpinnerModule,
		FileUploadModule
	]
})

export class SharedModule { }
