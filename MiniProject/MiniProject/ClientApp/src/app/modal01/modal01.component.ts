import { Component, OnInit } from '@angular/core';
import { StockInfoService } from '../stock-info.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-modal01',
  templateUrl: './modal01.component.html',
  styleUrls: ['./modal01.component.css']
})
export class Modal01Component implements OnInit {
  constructor(private activeModal: NgbActiveModal, private dataSvc: StockInfoService) {
  }
  message!: { stockId: string, stockName: string, stockPrice: string, stockDate: string };
  pageInfo!: string;
  isStockInfo: boolean = false;
  isMessage: boolean = false;
  urlResult!: string;
  ngOnInit() {
    this.pageInfo = this.dataSvc.getPageMessage();
    if (this.pageInfo == "isStockInfo") {
      this.isStockInfo = true;
      this.isMessage = false;
      this.message = this.dataSvc.getMessage();
    }
    else if (this.pageInfo == "isMessage") {
      this.isStockInfo = false;
      this.isMessage = true;
      this.urlResult = this.dataSvc.getUrlResultMessage();
    }
    
  }

  confirm() {
    this.activeModal.close(true);
  }
}
