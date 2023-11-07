import { Injectable } from '@angular/core'

import { Observable } from 'rxjs'
import { AccountService } from '../services/account.service'
import { ToastrService } from 'ngx-toastr'
import { map } from 'rxjs/operators'

@Injectable({
	providedIn: 'root'
})
export class AdminGuard  {

	constructor(private accountService: AccountService, private toastr: ToastrService) { }

	canActivate(): Observable<boolean> {
		return this.accountService.currentUser$.pipe(
			map(user => {
				if (user.roles.includes('Admin') || user.roles.includes('Moderator'))
					return true

				this.toastr.error('You cannot enter this area')
			})
		)
	}
}
