import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { ReplaySubject } from 'rxjs'
import { map } from 'rxjs/operators'
import { nameOf } from '../utils/helpers'
import { setItemToLocalStorage, removeItemFromLocalStorage } from '../utils/localStorage'
import User from '../_models/user'
import { environment } from 'src/environments/environment'
import { PresenceService } from './presence.service'

@Injectable({
  	providedIn: 'root'
})

export class AccountService {
	private baseUrl = environment.apiUrl
	private currentUserSource = new ReplaySubject<User>(1)
	currentUser$ = this.currentUserSource.asObservable()

  	constructor(private http: HttpClient, private presence: PresenceService) { }

	setCurrentUser(user: User) {
		user.roles = []
		const roles = this.getDecodedToken(user.token).role
		Array.isArray(roles) ? user.roles = roles : user.roles.push(roles)
		setItemToLocalStorage(nameOf(() => user), user)
		this.currentUserSource.next(user)
	}

	login(model: any) {
		return this.http.post(`${this.baseUrl}/account/login`, model)
			.pipe(
				map((user: User) => {
					if (user) {
						this.setCurrentUser(user)
						this.presence.createHubConnection(user)
					}
				})
			)
	}
	
	register(model: any) {
		return this.http.post(`${this.baseUrl}/account/register`, model)
			.pipe(
				map((user: User) => {
					if (user) {
						this.setCurrentUser(user)
						this.presence.createHubConnection(user)
					}
				})
			)
	}

	logout() {
		removeItemFromLocalStorage('user')
		this.currentUserSource.next(null)
		this.presence.stopHubConnection()
	}

	getDecodedToken(token: string) {
		return JSON.parse(atob(token.split('.')[1]))
	}
}
