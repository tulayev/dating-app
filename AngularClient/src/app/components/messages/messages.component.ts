import { Component, OnInit } from '@angular/core'
import { Message, MessageContainer } from '../../models/message'
import { Pagination } from '../../models/pagination'
import { MessageService } from '../../services/message.service'
import { PageChangedEvent } from 'ngx-bootstrap/pagination'

@Component({
	selector: 'app-messages',
	templateUrl: './messages.component.html',
	styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
	messages: Message[] = []
	pagination: Pagination
	messageContainer: MessageContainer = 'Unread'
	pageNumber = 1
	pageSize = 5
	loading = false

	constructor(private messageService: MessageService) { }

	ngOnInit(): void {
		this.loadMessages()
	}

	loadMessages() {
		this.loading = true

		this.messageService.getMessages(this.pageNumber, this.pageSize, this.messageContainer).subscribe(response => {
            console.log(response)
			this.messages = response.result
			this.pagination = response.pagination
			this.loading = false
		})
	}

	deleteMessage(id: number) {
		this.messageService.deleteMessage(id)
			.subscribe(() => this.messages.splice(this.messages.findIndex(x => x.id === id), 1))
	}

	pageChanged(event: PageChangedEvent) {
		if (this.pageNumber !== event.page) {
			this.pageNumber = event.page
			this.loadMessages()
		}
	}
}
