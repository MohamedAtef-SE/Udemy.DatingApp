
export interface PaginationHeader{
    currentPage: number,
    itemsPerPage: number,
    totalItems: number,
    totalPages:number
}

export class PaginatedResult<T>{
    items?: T[];
    paginationHeader?: PaginationHeader
}