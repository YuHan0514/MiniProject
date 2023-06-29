import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StockInfoService {
  msg!: { stockId: string, stockName: string, stockPrice: string ,stockDate:string};
  constructor() { }
  setMessage(value: any) {
    this.msg = value;
  }

  getMessage() {
    return this.msg;
  }
}
