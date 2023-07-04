import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { StockInfoService } from '../stock-info.service';
import { Modal01Component } from '../modal01/modal01.component';

@Component({
  selector: 'app-edit-stock',
  templateUrl: './edit-stock.component.html',
  styleUrls: ['./edit-stock.component.css']
})
export class EditStockComponent implements OnInit{
  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router, private ngbModal: NgbModal, private dataSvc: StockInfoService) { }
  stock!: { id: number; tradeDate: string; stockId: string; name: string; type: string; volume: number; fee: Float32Array; price: Float32Array; lendingPeriod: number; returnDate: string; };

  isEdit: boolean = false


  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      var id = Number(params.get('id'))
      this.getStockById(id);

    });
  }

  getStockById(id: number) {
    let url = "https://localhost:44320/Trade//GetStockById";
    this.http.post(url, id)
      .subscribe((res: any) => {
        this.stock = res
        this.isEdit = true
      })
  }
  goBack(): void {
    this.router.navigateByUrl('/stock');
  }
  editStockData() {
    if (this.stock.volume == null || this.stock.fee == null || this.stock.lendingPeriod == null || this.stock.returnDate == "") {
      this.dataSvc.setPageMessage("isMessage");
      this.dataSvc.setUrlResultMessage("輸入不可為空");
      this.ngbModal.open(Modal01Component);
    }
    else {
      let url = "https://localhost:44320/Trade//UpdateStockById";
      let data = this.stock;
      this.http.post(url, data)
        .subscribe((res: any) => {
          this.dataSvc.setPageMessage("isMessage");
          this.dataSvc.setUrlResultMessage(res.message);
          this.ngbModal.open(Modal01Component);
          this.goBack();
        })
    }
  }


}
