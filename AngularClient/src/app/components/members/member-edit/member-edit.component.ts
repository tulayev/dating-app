import { Component, HostListener, OnInit, ViewChild } from '@angular/core'
import { NgForm } from '@angular/forms'
import { ToastrService } from 'ngx-toastr'
import { take } from 'rxjs/operators'
import { Member } from 'src/app/models/member'
import User from 'src/app/models/user'
import { AccountService } from 'src/app/services/account.service'
import { MemberService } from 'src/app/services/member.service'

@Component({
	selector: 'app-member-edit',
	templateUrl: './member-edit.component.html',
	styleUrls: ['./member-edit.component.css']
})

export class MemberEditComponent implements OnInit {
	@ViewChild('editForm') editForm: NgForm
	@HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
		if (this.editForm.dirty)
			$event.returnValue = true
	}
	user: User
	member: Member

	constructor(private accountService: AccountService, private memberService: MemberService, private toastr: ToastrService) {
		this.accountService.currentUser$.pipe(take(1))
			.subscribe(user => this.user = user)
	}

	ngOnInit(): void {
		this.loadMember()
	}

	loadMember() {
		this.memberService.getMember(this.user.userName)
			.subscribe(member => this.member = member)
	}

	updateMember() {
		this.memberService.updateMember(this.member)
			.subscribe(() => {
				this.toastr.success('Updated successfully!')
				this.editForm.reset(this.member)
			})
	}
}
