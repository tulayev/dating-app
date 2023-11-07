import { ToastrService } from 'ngx-toastr';
import { Injectable } from '@angular/core'

import { Observable } from 'rxjs'
import { AccountService } from '../services/account.service'
import { map } from 'rxjs/operators'

@Injectable({
	providedIn: 'root'
})
export class AuthGuard  {
	constructor(private accountService: AccountService, private toastr: ToastrService) {}

	canActivate(): Observable<boolean> {
		return this.accountService.currentUser$.pipe(
			map(user => {
				if (user)
					return true

				this.toastr.error('You shall not pass!')
			})
		)
	}
}
