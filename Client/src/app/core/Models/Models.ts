


export interface ILoginForm{
    "username":string,
    "password":string
}

export interface IRegisterForm extends ILoginForm {
    "confirmPassword":string,
    "gender":string,
    "knownAs":string,
    "dateOfBirth":string
    "city":string,
    "country":string
}

export interface ICurrentUser{
    "username": string,
    "knownAs":string,
    "gender":string,
    "token": string,
    "photoURL"?: string,
    "roles"?:string[]
}

export interface IHttpErrorResponse{
    "headers": {
        "normalizedNames": {},
        "lazyUpdate": null,
        "headers": {}
    },
    "status": number,
    "statusText": string,
    "url": string,
    "ok": boolean,
    "name": string,
    "message": string,
    "error": {
        "type": string,
        "title": string,
        "status": number,
        "errors"?: {
            "Password"?: string[],
            "Username"?:string[]
        },
        "traceId": string
    }
}