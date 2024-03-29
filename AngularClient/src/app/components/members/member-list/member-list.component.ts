import { Component, OnInit } from '@angular/core'
import { PageChangedEvent } from 'ngx-bootstrap/pagination'
import { Member } from 'src/app/models/member'
import { Pagination } from 'src/app/models/pagination'
import { User } from 'src/app/models/user'
import { Gender, UserParams } from 'src/app/models/userParams'
import { MemberService } from 'src/app/services/member.service'

interface GenderDisplay {
    value: Gender
    display: string
}

@Component({
	selector: 'app-member-list',
	templateUrl: './member-list.component.html',
	styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
	members: Member[]
	pagination: Pagination
	user: User
	userParams: UserParams
	genderList: GenderDisplay[] = [
        { value: 'male', display: 'Males' }, 
        { value: 'female', display: 'Females' }
    ]

	constructor(private memberService: MemberService) {
		this.userParams = memberService.getUserParams()
	}

	ngOnInit(): void {
		this.loadMembers()
	}

	loadMembers() {
		this.memberService.setUserParams(this.userParams)

		this.memberService.getMembers(this.userParams).subscribe(response => {
			this.members = response.result
			this.pagination = response.pagination
		})
	}

	resetFilters() {
		this.userParams = this.memberService.resetUserParams()
		this.loadMembers()
	}

	pageChanged(event: PageChangedEvent) {
		this.userParams.pageNumber = event.page
		this.memberService.setUserParams(this.userParams)
		this.loadMembers()
	}
}
