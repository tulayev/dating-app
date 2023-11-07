import { User } from './user'

export type UserOrder = 'CreatedAt' | 'LastActive'

export type Gender = 'female' | 'male'

export class UserParams {
    pageNumber = 1
    pageSize = 5
    minAge = 18
    maxAge = 99
    gender: Gender
    orderBy: UserOrder = 'LastActive'

    constructor(user: User) {
        this.gender = user.gender === 'female' ? 'male' : 'female'
    }
}
