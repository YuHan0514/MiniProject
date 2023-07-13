import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { StockComponent } from './stock/stock.component';
import { FormsModule } from '@angular/forms';
import { Modal01Component } from './modal01/modal01.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Modal02Component } from './modal02/modal02.component';
import { EditStockComponent } from './edit-stock/edit-stock.component';
import { StockService } from './service/stock.service';

@NgModule({
  declarations: [
    AppComponent,
    StockComponent,
    Modal01Component,
    Modal02Component,
    EditStockComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    NgbModule
  ],
  providers: [StockService],
  bootstrap: [AppComponent]
})
export class AppModule { }
