import { Injectable } from '@angular/core';
import { Data } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class StockInfoService {
  msg!: { stockId: string, stockName: string, stockPrice: string, stockDate: string };
  endDate!: Date;
  pageInfo!: string;
  urlResult!: string;
  constructor() { }

  setUrlResultMessage(value: string) {
    this.urlResult = value;
  }

  getUrlResultMessage() {
    return this.urlResult;
  }

  setPageMessage(value: string) {
    this.pageInfo = value;
  }

  getPageMessage() {
    return this.pageInfo;
  }

  setDateMessage(value: Date) {
    this.endDate = value;
  }

  getDateMessage() {
    return this.endDate;
  }

  setMessage(value: any) {
    this.msg = value;
  }

  getMessage() {
    return this.msg;
  }
}
