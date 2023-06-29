import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Modal01Component } from '../modal01/modal01.component';
import { StockInfoService } from '../stock-info.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-stock',
  templateUrl: './stock.component.html',
  styleUrls: ['./stock.component.css']
})

export class StockComponent implements OnInit{
  constructor(private http: HttpClient, private dataSvc: StockInfoService, private ngbModal: NgbModal) { }
  stockArray: { tradeDate: string, stockId: string, name: string, type: string, volume: number, fee: Float32Array, price: Float32Array, lendingPeriod: number, returnDate: string }[] = [];
  message!:string;
  pageCount: number = 0;
  isFilter: boolean = false;
  isPages: boolean = false;
  startDate!: Date | null;
  endDate!: Date | null;
  selectedType: string = "";
  stockCode: string = "";
  ngOptions: number[] = [];
  selectedPage: number = 1;
  sortColumn: string = "Id";
  sortDirection: string = "↑";

  tradeDateDirection: string = "↑";
  stockCodeDirection: string = "－";
  typeDirection: string = "－";
  priceDirection: string = "－";
  volumeDirection: string = "－";
  feeDirection: string = "－";
  lendingPerioDirection: string = "－";
  returnDateDirection: string = "－";

  private getHeaders() {
    let headers = new HttpHeaders();
    headers = headers.append('content-type', 'application/json')
    return headers
  }

  addPageOption() {
    this.ngOptions = []
    for (let i = 1; i <= this.pageCount; i++) {
      this.ngOptions.push(i);
    }
  }

  getDataFromBackEndByPage() {
    this.getDataFromBackEnd(this.selectedPage, this.sortColumn, this.sortDirection);
  }

  sortData(tittle: string, direction: string) {
    switch (direction) {
      case "↑":
        direction = "↓";
        break;
      case "↓":
        direction = "↑";
        break;
      case "－":
        direction = "↑";
        break;
    }
    this.iniDirection();
    switch (tittle) {
      case "TradeDate":
        this.tradeDateDirection = direction;
        break;
      case "StockId":
        this.stockCodeDirection = direction;
        break;
      case "Type":
        this.typeDirection = direction;
        break;
      case "Price":
        this.priceDirection = direction;
        break;
      case "Volume":
        this.volumeDirection = direction;
        break;
      case "Fee":
        this.feeDirection = direction;
        break;
      case "LendingPeriod":
        this.lendingPerioDirection = direction;
        break;
      case "ReturnDate":
        this.returnDateDirection = direction;
        break;
    }
    this.getDataFromBackEnd(1, tittle, direction);
  }
  iniDirection() {
    this.tradeDateDirection = "－";
    this.stockCodeDirection = "－";
    this.typeDirection = "－";
    this.priceDirection = "－";
    this.volumeDirection = "－";
    this.feeDirection = "－";
    this.lendingPerioDirection = "－";
    this.returnDateDirection = "－";
  }

  getStockInfo(stockId: string, searchDate: string, stockName: string) {
    let url = "https://localhost:44320/Stock/GetStockInfo";
    let sendData = {
      stockId: stockId,
      searchDate: searchDate
    };
    this.http.post(url, sendData, { headers: this.getHeaders() }).subscribe((res: any) => {
      this.message = res

      let stockInfo = {
        stockId: stockId,
        stockName: stockName,
        stockPrice: this.message,
        stockDate: searchDate
      }
      this.dataSvc.setMessage(stockInfo);
      this.ngbModal.open(Modal01Component);
    })
    
  }

  getDataFromBackEnd(pageIndex: number, tittle: string, direction: string) {
    if (tittle == "Id") {
      this.iniDirection();
      this.tradeDateDirection = "↑";
    }
    this.selectedPage = pageIndex;
    this.sortColumn = tittle;
    this.sortDirection = direction;
    let url = "https://localhost:44320/Trade/GetStockListFromDB";
    let requestBody = {
      pageIndex: pageIndex,
      startDate: this.startDate,
      endDate: this.endDate,
      tradeType: this.selectedType,
      stockId: this.stockCode,
      sortColumn: this.sortColumn,
      sortDirection: this.sortDirection
    };
    this.http.post(url, requestBody, { headers: this.getHeaders() }).subscribe((res: any) => {
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
