import { AccountService } from './_services/account.service'
import { Component, OnInit } from '@angular/core'
import { getItemFromLocalStorage } from './utils/localStorage'
import User from './_models/user'

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
	title = 'Dating App'
	users: any

	constructor(private accountService: AccountService) {}

	ngOnInit() {
		this.setCurrentUser()
	}

	setCurrentUser() {
		const user: User = getItemFromLocalStorage('user')
		this.accountService.setCurrentUser(user)
	}
}
