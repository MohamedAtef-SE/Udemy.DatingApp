import { HttpParams, HttpResponse } from "@angular/common/http";
import { WritableSignal } from "@angular/core";
import { PaginatedResult } from "../Models/Pagination";
import { PaginationParams } from "../Models/paginationParams";



  export function setPaginatedResponse<T>(response:HttpResponse<T[]>,
    paginatedResult:WritableSignal<PaginatedResult<T> | null>){
    paginatedResult.set({
      paginationHeader: JSON.parse(response.headers.get('Pagination')!),
      items: response.body as T[]
    })
  }

  
  export function setPaginationHeader(paginationParams:PaginationParams) : HttpParams {
    let params = new HttpParams();
    let pageNumber = paginationParams.pageNumber;
    let pageSize = paginationParams.pageSize;

    if( pageNumber && pageSize){
      params = params.append("pageNumber",pageNumber);
      params = params.append("pageSize",pageSize);
    }
    return params;
  }