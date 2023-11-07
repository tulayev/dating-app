import { HttpClient, HttpParams } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { environment } from 'src/environments/environment'
import { getPaginatedResult } from '../utils/paginationHelper'
import { Message } from '../models/message'
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'
import { User } from '../models/user'
import { BehaviorSubject, take } from 'rxjs'
import { Group } from '../models/group'

@Injectable({
	providedIn: 'root'
})
export class MessageService {
	private baseUrl = environment.apiUrl
	private hubUrl = environment.hubUrl
	private hubConnection: HubConnection
	private messageThreadSource = new BehaviorSubject<Message[]>([])
	messageThread$ = this.messageThreadSource.asObservable()

	constructor(private http: HttpClient) { }

	createHubConnection(user: User, otherUserName: string) {
		this.hubConnection = new HubConnectionBuilder()
			.withUrl(`${this.hubUrl}/message?user=${otherUserName}`, {
				accessTokenFactory: () => user.token
			})
			.withAutomaticReconnect()
			.build()

		this.hubConnection.start().catch(err => console.log(err))

		this.hubConnection.on('RecieveMessageThread', messages => {
			this.messageThreadSource.next(messages)
		})

		this.hubConnection.on('NewMessage', message => {
			this.messageThread$.pipe(take(1)).subscribe(messages => {
				this.messageThreadSource.next([...messages, message])
			})
		})

		this.hubConnection.on('UpdatedGroup', (group: Group) => {
			if (group.connections.some(x => x.userName === otherUserName)) {
				this.messageThread$.pipe(take(1)).subscribe(messages => {
					messages.forEach(message => {
						if (!message.dateRead) {
							message.dateRead = new Date(Date.now())
						}
					})

					this.messageThreadSource.next([...messages])
				})
			}
		})
	}

	stopHubConnection() {
		if (this.hubConnection)
			this.hubConnection.stop()
	}

	getMessages(pageNumber: number, pageSize: number, container: string) {
		let params = new HttpParams()
		params = params.append('pageNumber', pageNumber.toString())
		params = params.append('pageSize', pageSize.toString())
		params = params.append('messageContainer', container)

		return getPaginatedResult<Message[]>(`${this.baseUrl}/messages`, params, this.http)
	}

	getMessageThread(username: string) {
		return this.http.get<Message[]>(`${this.baseUrl}/messages/thread/${username}`)
	}

	async sendMessage(username: string, content: string) {
		return this.hubConnection.invoke('SendMessage', { recipientUserName: username, content })
			.catch(err => console.log(err))
	}

	deleteMessage(id: number) {
		return this.http.delete(`${this.baseUrl}/messages/${id}`)
	}
}
