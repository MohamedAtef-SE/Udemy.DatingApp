import { IUser } from "./Models";
import { PaginationParams } from "./paginationParams";


export class UserParams extends PaginationParams {
    gender:string;
    minAge = 18;
    maxAge = 99;
    city: string | undefined = undefined;
    country: string | undefined = undefined; 
    orderBy: string = 'lastActive'

    constructor(User:IUser){
        super();
        this.gender = User.gender == 'female' ? 'male' : 'female'
    }
}