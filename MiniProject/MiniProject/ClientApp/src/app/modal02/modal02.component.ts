import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-modal02',
  templateUrl: './modal02.component.html',
  styleUrls: ['./modal02.component.css']
})
export class Modal02Component {
  constructor(private activeModal: NgbActiveModal) {
  }
  confirm() {
    this.activeModal.close(true);
  }

  cancell() {
    this.activeModal.close(false);
  }
}
