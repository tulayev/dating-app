import { HttpClient, HttpParams } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { environment } from 'src/environments/environment'
import { Member } from '../models/member'
import { of } from 'rxjs'
import { map, take } from 'rxjs/operators'
import { UserParams } from '../models/userParams'
import { AccountService } from './account.service'
import { User } from '../models/user'
import { getPaginatedResult } from '../utils/paginationHelper'

@Injectable({
	providedIn: 'root'
})
export class MemberService {
	baseUrl = environment.apiUrl
	members: Member[] = []
	memberCache = new Map()
	user: User
	userParams: UserParams

  	constructor(private http: HttpClient, private accountService: AccountService) {
		this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
			this.user = user
			this.userParams = new UserParams(user)
		})
	}

	getUserParams() {
		return this.userParams
	}

	setUserParams(params: UserParams) {
		this.userParams = params
	}

	resetUserParams() {
		this.userParams = new UserParams(this.user)
		return this.userParams
	}

	getMembers(userParams: UserParams) {
		var response = this.memberCache.get(Object.values(userParams).join('-'))

		if (response)
			return of(response)

		let params = new HttpParams()
		params = params.append('pageNumber', userParams.pageNumber.toString())
		params = params.append('pageSize', userParams.pageSize.toString())
		params = params.append('minAge', userParams.minAge.toString())
		params = params.append('maxAge', userParams.maxAge.toString())
		params = params.append('gender', userParams.gender)
		params = params.append('orderBy', userParams.orderBy)

		return getPaginatedResult<Member[]>(`${this.baseUrl}/users`, params, this.http)
			.pipe(map(response => {
				this.memberCache.set(Object.values(userParams).join('-'), response)
				return response
			}))
	}

	getMember(username: string) {
		const member = [...this.memberCache.values()]
			.reduce((arr, elem) => arr.concat(elem.result), [])
			.find((member: Member) => member.userName === username)

		if (member)
			return of(member)

		return this.http.get<Member>(`${this.baseUrl}/users/${username}`)
	}

	updateMember(member: Member) {
		return this.http.put(`${this.baseUrl}/users`, member).pipe(
			map(() => {
				const index = this.members.indexOf(member)
				this.members[index] = member
			})
		)
	}

	setMainPhoto(photoId: number) {
		return this.http.put(`${this.baseUrl}/users/set-main-photo/${photoId}`, {})
	}

	deletePhoto(photoId: number) {
		return this.http.delete(`${this.baseUrl}/users/delete-photo/${photoId}`)
	}

	addLike(username: string) {
		return this.http.post(`${this.baseUrl}/likes/${username}`, {})
	}

	getLikes(predicate: string, pageNumber: number, pageSize: number) {
		let params = new HttpParams()
		params = params.append('pageNumber', pageNumber.toString())
		params = params.append('pageSize', pageSize.toString())
		params = params.append('predicate', predicate)

		return getPaginatedResult<Partial<Member[]>>(`${this.baseUrl}/likes`, params, this.http)
	}
}