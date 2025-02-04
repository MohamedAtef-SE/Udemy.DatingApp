import { PaginationParams } from "./paginationParams";



export class LikesParams extends PaginationParams{

    predicate:string;
    constructor(){
        super();
        this.predicate = 'liked';
    }
    
}