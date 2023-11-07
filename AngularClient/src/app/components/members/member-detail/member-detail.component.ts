import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core'
import { ActivatedRoute, Router } from '@angular/router'
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery'
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs'
import { take } from 'rxjs'
import { Member } from 'src/app/models/member'
import { Message }from 'src/app/models/message'
import { User } from 'src/app/models/user'
import { AccountService } from 'src/app/services/account.service'
import { MessageService } from 'src/app/services/message.service'
import { PresenceService } from 'src/app/services/presence.service'

@Component({
	selector: 'app-member-detail',
	templateUrl: './member-detail.component.html',
	styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit, OnDestroy {
	@ViewChild('memberTabs', { static: true }) memberTabs: TabsetComponent
	user: User
	member: Member
	messages: Message[] = []
	galleryOptions: NgxGalleryOptions[]
  	galleryImages: NgxGalleryImage[]
	activeTab: TabDirective

	constructor(private route: ActivatedRoute, private messageService: MessageService,
		private accountService: AccountService, private router: Router, public presenceService: PresenceService) 
    {
		this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user)
		//this.router.routeReuseStrategy()
	}

	ngOnInit(): void {
		this.route.data.subscribe(data => this.member = data.member)

		this.route.queryParams.subscribe(params => {
			params.tab ? this.selectTab(params.tab) : this.selectTab(0)
		})

		this.galleryOptions = [
			{
				width: '500px',
				height: '500px',
				imagePercent: 100,
				thumbnailsColumns: 4,
				imageAnimation: NgxGalleryAnimation.Slide,
				preview: false
			}
		]

		this.galleryImages = this.loadImages()
	}

	ngOnDestroy(): void {
		this.messageService.stopHubConnection()
	}

	loadImages(): NgxGalleryImage[] {
		const imageUrls = []

		for (const photo of this.member.photos) {
			imageUrls.push({
				small: photo?.url,
				medium: photo?.url,
				big: photo?.url
			})
		}

		return imageUrls
	}

	loadMessages() {
		this.messageService.getMessageThread(this.member.userName)
			.subscribe(messages => this.messages = messages)
	}

	selectTab(tabIndex: number) {
		this.memberTabs.tabs[tabIndex].active = true
	}

	onTabActivated(data: TabDirective) {
		this.activeTab = data

		if (this.activeTab.heading == 'Messages' && this.messages.length === 0)
			this.messageService.createHubConnection(this.user, this.member.userName)
		else
			this.messageService.stopHubConnection()
	}
}
