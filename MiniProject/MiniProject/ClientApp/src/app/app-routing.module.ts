import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EditStockComponent } from './edit-stock/edit-stock.component';
import { StockComponent } from './stock/stock.component';

const routes: Routes = [
  { path: '', redirectTo: '/stock', pathMatch: 'full' },
  { path: 'stock', component: StockComponent },
  { path: 'edit/:id', component: EditStockComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
