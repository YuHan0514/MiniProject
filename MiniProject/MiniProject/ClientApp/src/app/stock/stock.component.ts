import { Component, OnInit } from '@angular/core';
import { Modal02Component } from '../modal02/modal02.component';
import { StockInfoService } from '../stock-info.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { StockService } from '../service/stock.service';
@Component({
  selector: 'app-stock',
  templateUrl: './stock.component.html',
  styleUrls: ['./stock.component.css']
})

export class StockComponent implements OnInit {
  constructor( private dataSvc: StockInfoService, private ngbModal: NgbModal, private stockService: StockService) { }
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

  ngOnInit(): void {
    let currentDate = new Date();
    let year = currentDate.getFullYear();
    let month = ('0' + (currentDate.getMonth() + 1)).slice(-2);
    let day = ('0' + currentDate.getDate()).slice(-2);
    this.endDate = `${year}-${month}-${day}`;
  }


  getDataFromBackEndByPage() {
    this.getDataFromBackEnd(this.selectedPage, this.sortColumn, this.sortDirection);
  }

  async sortData(tittle: string, direction: string) {
    let data = await this.stockService.sortData(tittle, direction)
    this.sortColumn = tittle
    this.stockArray = data.stockArray
    this.pageCount = data.pageCount
    this.totalCount = data.totalCount
    this.isFilter = true;
    this.addPageOption();
    this.isPages = true;
    this.getDirectionStatus()
    this.selectedPage = 1;
    this.sortDirection = this.stockService.sortDirection

  }

  getDirectionStatus() {
    this.tradeDateDirection = this.stockService.tradeDateDirection
    this.stockCodeDirection = this.stockService.stockCodeDirection
    this.typeDirection = this.stockService.typeDirection
    this.priceDirection = this.stockService.priceDirection
    this.volumeDirection = this.stockService.volumeDirection
    this.feeDirection = this.stockService.feeDirection
    this.lendingPerioDirection = this.stockService.lendingPerioDirection
    this.returnDateDirection = this.stockService.returnDateDirection
  }

  async insertTwseDataToDB() {
    this.isButtonDisabled = true;
    this.dataSvc.setPageMessage("isUpdateDB");
    const modal = this.ngbModal.open(Modal02Component);
     modal.result.then(
      async (result) => {
         if (result == true) {
           let status = await this.stockService.insertTwseDataToDB();
           this.isButtonDisabled = status;
         }
         else {
           this.isButtonDisabled = false;
         }
      })
  }

  getStockInfo(stockId: string, searchDate: string, stockName: string) {
    this.stockService.getStockInfo(stockId, searchDate, stockName);

  }

  async getDataFromBackEnd(pageIndex: number, tittle: string, direction: string) {
    this.sortColumn = tittle
    let data = await this.stockService.getDataFromBackEnd(pageIndex, tittle, direction, this.endDate, this.startDate, this.selectedType, this.stockCode)
    this.stockArray = data.stockArray
    this.pageCount = data.pageCount
    this.totalCount = data.totalCount
    this.selectedPage = pageIndex;
    this.isFilter = true;
    this.addPageOption();
    this.isPages = true;
    this.getDirectionStatus()

  }
  addPageOption() {
    this.ngOptions = []
    for (let i = 2; i <= this.pageCount; i++) {
      this.ngOptions.push(i);
    }
  }
  deleteStock(id: number) {
    this.stockService.deleteStock(id);

  }
}
