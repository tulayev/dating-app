import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { ReplaySubject } from 'rxjs'
import { map } from 'rxjs/operators'
import { nameOf } from '../utils/helpers'
import { setItemToLocalStorage, removeItemFromLocalStorage } from '../utils/localStorage'
import User from '../_models/user'

@Injectable({
  	providedIn: 'root'
})
export class AccountService {
	baseUrl = 'http://localhost:5000/api'
	private currentUserSource = new ReplaySubject<User>(1)
	currentUser$ = this.currentUserSource.asObservable()

  	constructor(private http: HttpClient) { }

	login(model: any) {
		return this.http.post(`${this.baseUrl}/account/login`, model)
			.pipe(
				map((user: User) => {
					if (user) {
						setItemToLocalStorage(nameOf(() => user), user)
						this.currentUserSource.next(user)
					}
				})
			)
	}
	
	register(model: any) {
		return this.http.post(`${this.baseUrl}/account/register`, model)
			.pipe(
				map((user: User) => {
					if (user) {
						setItemToLocalStorage(nameOf(() => user), user)
						this.currentUserSource.next(user)
					}
				})
			)
	}

	setCurrentUser(user: User) {
		this.currentUserSource.next(user)
	}

	logout() {
		removeItemFromLocalStorage('user')
		this.currentUserSource.next(null)
	}
}
