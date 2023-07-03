import { Component, OnInit } from '@angular/core';
import { StockInfoService } from '../stock-info.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-modal02',
  templateUrl: './modal02.component.html',
  styleUrls: ['./modal02.component.css']
})
export class Modal02Component implements OnInit{
  constructor(private activeModal: NgbActiveModal, private dataSvc: StockInfoService) {
  }
  isDelete: boolean = false;
  isUpdateDB: boolean = false;
  pageInfo!: string;
  endDate!: Date;
  ngOnInit() {
    this.pageInfo = this.dataSvc.getPageMessage();
    if (this.pageInfo == "isDelete") {
      this.isDelete = true;
      this.isUpdateDB = false;
    }
    else if (this.pageInfo == "isUpdateDB") {
      this.isDelete = false;
      this.isUpdateDB = true;
    }

  }

  setMessage() {
    this.dataSvc.setDateMessage(this.endDate);
  }

  confirm() {
    this.activeModal.close(true);
    this.setMessage();
  }

  cancell() {
    this.activeModal.close(false);
  }
}
