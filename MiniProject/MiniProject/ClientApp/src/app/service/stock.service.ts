import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Modal01Component } from '../modal01/modal01.component';
import { Modal02Component } from '../modal02/modal02.component';
import { StockInfoService } from '../stock-info.service';
import { DataModel } from '../data-model';

@Injectable({
  providedIn: 'root'
})
export class StockService {
  dataModel: DataModel;
  constructor(private http: HttpClient, private dataSvc: StockInfoService, private ngbModal: NgbModal) {
    this.dataModel = new DataModel();
  }

  getHeaders() {
    let headers = new HttpHeaders();
    headers = headers.append('content-type', 'application/json')
    return headers
  }

  getDataFromBackEndByPage() {
    this.getDataFromBackEnd(this.dataModel.selectedPage, this.dataModel.sortColumn, this.dataModel.sortDirection, this.dataModel.endDate, this.dataModel.startDate, this.dataModel.selectedType, this.dataModel.stockCode);
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
        this.dataModel.tradeDateDirection = direction;
        break;
      case "StockId":
        this.dataModel.stockCodeDirection = direction;
        break;
      case "Type":
        this.dataModel.typeDirection = direction;
        break;
      case "Price":
        this.dataModel.priceDirection = direction;
        break;
      case "Volume":
        this.dataModel.volumeDirection = direction;
        break;
      case "Fee":
        this.dataModel.feeDirection = direction;
        break;
      case "LendingPeriod":
        this.dataModel.lendingPerioDirection = direction;
        break;
      case "ReturnDate":
        this.dataModel.returnDateDirection = direction;
        break;
    }
    let returnData = {
      sortDirection: direction,
      sortColumn: tittle,
      tradeDateDirection: this.dataModel.tradeDateDirection,
      stockCodeDirection: this.dataModel.stockCodeDirection,
      typeDirection: this.dataModel.typeDirection,
      priceDirection: this.dataModel.priceDirection,
      volumeDirection: this.dataModel.volumeDirection,
      feeDirection: this.dataModel.feeDirection,
      lendingPerioDirection: this.dataModel.lendingPerioDirection,
      returnDateDirection: this.dataModel.returnDateDirection
    }
    return returnData
  }

  iniDirection() {
    this.dataModel.tradeDateDirection = "－";
    this.dataModel.stockCodeDirection = "－";
    this.dataModel.typeDirection = "－";
    this.dataModel.priceDirection = "－";
    this.dataModel.volumeDirection = "－";
    this.dataModel.feeDirection = "－";
    this.dataModel.lendingPerioDirection = "－";
    this.dataModel.returnDateDirection = "－";
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
  }

  getStockInfo(stockId: string, searchDate: string, stockName: string) {
    let url = "https://localhost:44320/Stock/GetStockInfo";
    let sendData = {
      stockId: stockId,
      searchDate: searchDate
    };
    this.http.post(url, sendData, { headers: this.getHeaders() }).subscribe((res: any) => {
      this.dataModel.message = res

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
    this.dataModel.endDate = endDate;
    this.dataModel.selectedPage = pageIndex;
    this.dataModel.sortColumn = tittle;
    this.dataModel.sortDirection = direction;
    this.dataModel.startDate = startDate;
    this.dataModel.selectedType = selectedType;
    this.dataModel.stockCode = stockCode;
    let url = "https://localhost:44320/Trade/GetStockListFromDB";
    let requestBody = {
      pageIndex: pageIndex,
      startDate: this.dataModel.startDate,
      endDate: this.dataModel.endDate,
      tradeType: this.dataModel.selectedType,
      stockId: this.dataModel.stockCode,
      sortColumn: this.dataModel.sortColumn,
      sortDirection: this.dataModel.sortDirection
    };
    const response = await this.http.post(url, requestBody, { headers: this.getHeaders() }).toPromise()
    const res: any = response;
    this.dataModel.stockArray = res.items
    this.dataModel.pageCount = res.totalPage
    this.dataModel.totalCount = res.totalCount
    this.dataModel.isFilter = true;
    this.dataModel.isPages = true;
    let returnData = {
      stockArray: this.dataModel.stockArray,
      pageCount: this.dataModel.pageCount,
      totalCount: this.dataModel.totalCount,
      selectedPage: this.dataModel.selectedPage
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
