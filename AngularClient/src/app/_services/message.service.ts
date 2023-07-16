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

	getMessageThread(username: string) {
		return this.http.get<Message[]>(`${this.baseUrl}/messages/thread/${username}`)
	}

	sendMessage(username: string, content: string) {
		return this.http.post<Message>(`${this.baseUrl}/messages`, { recipientUserName: username, content })
	}

	deleteMessage(id: number) {
		return this.http.delete(`${this.baseUrl}/messages/${id}`)
	}
}
