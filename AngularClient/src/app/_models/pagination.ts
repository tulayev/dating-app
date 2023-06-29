export interface Pagination {
    currentPage: number
    itemsPerPage: number
    totalItems: number
    totalPages: number
}

export default class PaginatedResult<T> {
    result: T
    pagination: Pagination
}