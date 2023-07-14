export class DataModel {
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
  constructor() {
    let currentDate = new Date();
    let year = currentDate.getFullYear();
    let month = ('0' + (currentDate.getMonth() + 1)).slice(-2);
    let day = ('0' + currentDate.getDate()).slice(-2);
    this.endDate = `${year}-${month}-${day}`;
  }
}
