import { HttpClient, HttpParams } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { environment } from 'src/environments/environment'
import { getPaginatedResult } from '../utils/paginationHelper'
import Message from '../_models/message'

@Injectable({
	providedIn: 'root'
})

export class MessageService {
	baseUrl = environment.apiUrl

	constructor(private http: HttpClient) { }

	getMessages(pageNumber: number, pageSize: number, container: string) {
		let params = new HttpParams()
		params = params.append('pageNumber', pageNumber.toString())
		params = params.append('pageSize', pageSize.toString())
		params = params.append('container', container)

		return getPaginatedResult<Message[]>(`${this.baseUrl}/messages`, params, this.http)
	}
}
