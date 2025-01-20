









export interface ILoginForm{
    "username":string,
    "password":string
}

export interface IRegisterForm extends ILoginForm{}

export interface ILoginResponse{
    "username": string,
    "token": string
}

export interface IRegisterResponse extends ILoginResponse {}

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