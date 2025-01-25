import { IPhoto } from "./IPhoto"


export interface IMember {
    id: number
    userName: string
    photoURL: string
    age: number
    knownAs: string
    created: Date
    lastActive: Date
    gender: string
    introduction: string
    interests: string
    lookingFor: string
    city: string
    country: string
    photos: IPhoto[]
  }