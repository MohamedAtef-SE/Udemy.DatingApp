import { PaginationParams } from "./paginationParams"



export interface IMessage{
    id: number,
    senderId: number,
    senderUserName: string,
    senderPhotoURL: string,
    recipientId: number,
    recipientUserName: string,
    recipientPhotoURL: string,
    content: string,
    dateRead?: Date,
    messageSent: Date
}

export class MessageParams extends PaginationParams{
    container: string = 'Inbox'
}