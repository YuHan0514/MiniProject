import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Component({
  selector: 'app-stock',
  templateUrl: './stock.component.html',
  styleUrls: ['./stock.component.css']
})

export class StockComponent implements OnInit{
  constructor(private http: HttpClient) { }
  stockArray: { tradeDate: string, stockId: string, name: string, type: string, volume: number, fee: Float32Array, price: Float32Array, lendingPeriod: number, returnDate: string }[] = []
  pageCount: number = 0;
  isFilter: boolean = false;
  startDate: string = "";
  isPages: boolean = false;

  ngOptions: number[] = [1];
  ngDropdown: number = this.ngOptions[1];
  addPageOption() {
    for (let i = 2; i <= this.pageCount; i++) {
      if (!this.ngOptions.includes(i))
        this.ngOptions.push(i);
    }
  }
  selectedPage: string = "";
  

  getDataFromBackEnd(startDate: string) {
    let pageIndex = 1;
    if (this.selectedPage != "")
      pageIndex = parseInt(this.selectedPage);
    let url = "https://localhost:44320/Trade/GetStockListFromDB";
    let headers = new HttpHeaders({
      'Content-Type': 'text/json'
    });
    let requestBody = {
      pageIndex: pageIndex,
      startDate: startDate
    };
    
    this.http.post(url,requestBody , { headers }).subscribe((res: any) => {
      this.stockArray = res.item1
      this.pageCount = res.item2
      this.isFilter = true;
      this.addPageOption();
      this.isPages = true;
    })
  }

  ngOnInit(): void {
    /*this.getDataFromBackEnd();*/
  }
}
