import { Injectable, signal, WritableSignal } from '@angular/core';
import { PaginatedResult } from '../Models/Pagination';
import { IMember } from '../Models/IMember';

@Injectable({
  providedIn: 'root'
})
export class PaginationService {
  paginatedResult:WritableSignal<PaginatedResult<IMember> | null> = signal(null)
}
