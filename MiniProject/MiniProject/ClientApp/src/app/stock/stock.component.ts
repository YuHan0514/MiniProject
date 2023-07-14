import { Component, OnInit } from '@angular/core';
import { Modal02Component } from '../modal02/modal02.component';
import { StockInfoService } from '../stock-info.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { StockService } from '../service/stock.service';
import { DataModel } from '../data-model';
@Component({
  selector: 'app-stock',
  templateUrl: './stock.component.html',
  styleUrls: ['./stock.component.css']
})

export class StockComponent implements OnInit {
  dataModel: DataModel;
  constructor(private dataSvc: StockInfoService, private ngbModal: NgbModal, private stockService: StockService) {
    this.dataModel = new DataModel();
  }

  ngOnInit(): void {
  }

  getDataFromBackEndByPage() {
    this.getDataFromBackEnd(this.dataModel.selectedPage, this.dataModel.sortColumn, this.dataModel.sortDirection);
  }

  async sortData(tittle: string, direction: string) {
    let init = await this.stockService.sortData(tittle, direction)
    await this.getDataFromBackEnd(1, tittle, init.sortDirection);
    this.addPageOption();
    this.dataModel.isPages = true;
    this.getDirectionStatus(init)
    this.dataModel.selectedPage = 1;
  }

  getDirectionStatus(init: any) {
    this.dataModel.sortDirection = init.sortDirection
    this.dataModel.sortColumn = init.sortColumn
    this.dataModel.tradeDateDirection = init.tradeDateDirection
    this.dataModel.stockCodeDirection = init.stockCodeDirection
    this.dataModel.typeDirection = init.typeDirection
    this.dataModel.priceDirection = init.priceDirection
    this.dataModel.volumeDirection = init.volumeDirection
    this.dataModel.feeDirection = init.feeDirection
    this.dataModel.lendingPerioDirection = init.lendingPerioDirection
    this.dataModel.returnDateDirection = init.returnDateDirection
  }

  async insertTwseDataToDB() {
    this.dataModel.isButtonDisabled = true;
    this.dataSvc.setPageMessage("isUpdateDB");
    const modal = this.ngbModal.open(Modal02Component);
     modal.result.then(
      async (result) => {
         if (result == true) {
           await this.stockService.insertTwseDataToDB();
           this.dataModel.isButtonDisabled = false;
         }
         else {
           this.dataModel.isButtonDisabled = false;
         }
      })
  }

  getStockInfo(stockId: string, searchDate: string, stockName: string) {
    this.stockService.getStockInfo(stockId, searchDate, stockName);
  }

  async getDataFromBackEnd(pageIndex: number, tittle: string, direction: string) {
    if (tittle == "Id") {
      let init = await this.stockService.sortData("TradeDate", "↓")
      this.getDirectionStatus(init)
    }
    this.dataModel.sortColumn = tittle
    let data = await this.stockService.getDataFromBackEnd(pageIndex, tittle, direction, this.dataModel.endDate, this.dataModel.startDate, this.dataModel.selectedType, this.dataModel.stockCode)
    this.dataModel.stockArray = data.stockArray
    this.dataModel.pageCount = data.pageCount
    this.dataModel.totalCount = data.totalCount
    this.dataModel.selectedPage = data.selectedPage
    this.addPageOption();
    this.dataModel.isPages = true;
  }
  addPageOption() {
    this.dataModel.ngOptions = []
    for (let i = 2; i <= this.dataModel.pageCount; i++) {
      this.dataModel.ngOptions.push(i);
    }
  }
  deleteStock(id: number) {
    this.stockService.deleteStock(id);
  }
}
