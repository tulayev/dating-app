import { AccountService } from './_services/account.service'
import { Component, OnInit } from '@angular/core'
import { getItemFromLocalStorage } from './utils/localStorage'
import User from './_models/user'
import { PresenceService } from './_services/presence.service'

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
	title = 'Dating App'
	users: any

	constructor(private accountService: AccountService, private presence: PresenceService) {}

	ngOnInit() {
		this.setCurrentUser()
	}

	setCurrentUser() {
		const user: User = getItemFromLocalStorage('user')

		if (user) {
			this.accountService.setCurrentUser(user)
			this.presence.createHubConnection(user)
		}
	}
}
