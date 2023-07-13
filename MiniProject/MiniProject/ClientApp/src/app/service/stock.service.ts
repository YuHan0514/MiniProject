import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Modal01Component } from '../modal01/modal01.component';
import { Modal02Component } from '../modal02/modal02.component';
import { StockInfoService } from '../stock-info.service';

@Injectable({
  providedIn: 'root'
})
export class StockService {

  constructor(private http: HttpClient, private dataSvc: StockInfoService, private ngbModal: NgbModal) { }
  stockArray: { id: number, tradeDate: string, stockId: string, name: string, type: string, volume: number, fee: number, price: number, lendingPeriod: number, returnDate: string }[] = [];
  message!: string;
  pageCount: number = 0;
  totalCount: number = 0;
  isFilter: boolean = false;
  isPages: boolean = false;
  startDate: string = "2023-01-01";
  endDate!: string;
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
  isButtonDisabled = false;

  getHeaders() {
    let headers = new HttpHeaders();
    headers = headers.append('content-type', 'application/json')
    return headers
  }



  getDataFromBackEndByPage() {
    this.getDataFromBackEnd(this.selectedPage, this.sortColumn, this.sortDirection, this.endDate, this.startDate, this.selectedType, this.stockCode);
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
    let returnData = this.getDataFromBackEnd(1, tittle, direction, this.endDate, this.startDate, this.selectedType, this.stockCode);
    this.sortDirection = direction
    return returnData
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

  async insertTwseDataToDB(): Promise<any> {
    
    let url = "https://localhost:44320/Trade/InsertTwseDataToDB";
    let endDate = this.dataSvc.getDateMessage();
    let data = JSON.stringify(endDate);
    const response = await this.http.post(url, data, { headers: this.getHeaders() }).toPromise()
    const res: any = response;
    this.dataSvc.setPageMessage("isMessage");
    this.dataSvc.setUrlResultMessage(res.message);
    this.ngbModal.open(Modal01Component);
    return false
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
        stockPrice: res.price,
        stockDate: res.tradeDate
      }
      this.dataSvc.setPageMessage("isStockInfo");
      this.dataSvc.setMessage(stockInfo);
      this.ngbModal.open(Modal01Component);
    })

  }

  async getDataFromBackEnd(pageIndex: number, tittle: string, direction: string, endDate: string, startDate: string, selectedType: string, stockCode:string): Promise<any> {
    if (tittle == "Id") {
      this.iniDirection();
      this.tradeDateDirection = "↑";
     }
     this.endDate = endDate;
    this.selectedPage = pageIndex;
    this.sortColumn = tittle;
    this.sortDirection = direction;
    this.startDate = startDate;
    this.selectedType = selectedType;
    this.stockCode = stockCode;
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
    const response = await this.http.post(url, requestBody, { headers: this.getHeaders() }).toPromise()
    const res: any = response;
    this.stockArray = res.items
    this.pageCount = res.totalPage
    this.totalCount = res.totalCount
    this.isFilter = true;
    this.isPages = true;
    let returnData = {
      stockArray: this.stockArray,
      pageCount: this.pageCount,
      totalCount: this.totalCount
    }
    return returnData
  }
  deleteStock(id: number) {
    this.dataSvc.setPageMessage("isDelete");
    const modal = this.ngbModal.open(Modal02Component);
    let url = "https://localhost:44320/Trade/DeleteStockByStatus";
    modal.result.then(
      (result) => {
        if (result == true) {
          this.http.post(url, id, { headers: this.getHeaders() }).subscribe((res: any) => {
            this.getDataFromBackEndByPage();
          })
        }
      }
    )
  }

}
