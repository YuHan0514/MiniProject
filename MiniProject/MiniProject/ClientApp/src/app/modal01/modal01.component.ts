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

  ngOnInit() {
    this.message = this.dataSvc.getMessage();
  }

  confirm() {
    /*alert(Text);*/
    this.activeModal.close(true);
  }
}
