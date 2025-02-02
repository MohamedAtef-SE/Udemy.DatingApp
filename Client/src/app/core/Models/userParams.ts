import { ICurrentUser } from "./Models";

export class UserParams {
    gender:string;
    minAge = 18;
    maxAge = 99;
    pageNumber = 1;
    pageSize = 5;
    city: string | undefined = undefined;
    country: string | undefined = undefined; 
    orderBy: string = 'lastActive'

    constructor(User:ICurrentUser){
        this.gender = User.gender == 'female' ? 'male' : 'female'
    }
}