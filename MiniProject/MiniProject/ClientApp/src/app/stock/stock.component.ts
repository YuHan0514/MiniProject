import { Component, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";

@Component({
  selector: 'app-stock',
  templateUrl: './stock.component.html',
  styleUrls: ['./stock.component.css']
})
export class StockComponent implements OnInit{
  constructor(private http: HttpClient) { }
  stockArray: { tradeDate: string, stockId: string, name: string, type: string, volume: number, fee: Float32Array, price: Float32Array, lendingPeriod: number, returnDate: string }[] = []

  getDataFromBackEnd() {
    let url = "https://localhost:44320/Trade/GetDataFromDB";
    this.http.get(url).subscribe((res: any) => {
      this.stockArray = res
    })
  }

  ngOnInit(): void {
    this.getDataFromBackEnd();
  }
}
