import { ToastrService } from 'ngx-toastr'
import { Component, EventEmitter, OnInit, Output } from '@angular/core'
import { AccountService } from '../../services/account.service'
import { AbstractControl, UntypedFormBuilder, UntypedFormGroup, ValidatorFn, Validators } from '@angular/forms'
import { Router } from '@angular/router'

@Component({
	selector: 'app-register',
	templateUrl: './register.component.html',
	styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
	@Output() cancelRegister = new EventEmitter<boolean>()
	registerForm: UntypedFormGroup
	maxDate: Date
	validationErrors: string[] = []

	constructor(private accountService: AccountService, private toastr: ToastrService, private fb: UntypedFormBuilder, private router: Router) { }

	ngOnInit(): void {
		this.initForm()
		this.maxDate = new Date()
		this.maxDate.setFullYear(this.maxDate.getFullYear() - 18)
	}

	initForm() {
		this.registerForm = this.fb.group({
			gender: ['male'],
			knownAs: ['', Validators.required],
			dateOfBirth: ['', Validators.required],
			city: ['', Validators.required],
			country: ['', Validators.required],
			username: ['', Validators.required],
			password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
			confirmPassword: ['', [Validators.required, this.matchValues('password')]]
		})

		this.registerForm.controls.password.valueChanges.subscribe(() => {
			this.registerForm.controls.confirmPassword.updateValueAndValidity()
		})
	}

	matchValues(matchTo: string): ValidatorFn {
		return (control: AbstractControl) => {
			return control?.value === control?.parent?.controls[matchTo].value ? null : { isMatching: true }
		}
	}

	register() {
		this.accountService.register(this.registerForm.value)
			.subscribe({
                next: () => this.router.navigateByUrl('/members'),
                error: (error) => this.validationErrors = error
            })
	}

	cancel() {
		this.cancelRegister.emit(false)
	}
}
