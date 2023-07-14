"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.DataModel = void 0;
var DataModel = /** @class */ (function () {
    function DataModel() {
        this.stockArray = [];
        this.pageCount = 0;
        this.totalCount = 0;
        this.isFilter = false;
        this.isPages = false;
        this.startDate = "2023-01-01";
        this.selectedType = "";
        this.stockCode = "";
        this.ngOptions = [];
        this.selectedPage = 1;
        this.sortColumn = "Id";
        this.sortDirection = "↑";
        this.tradeDateDirection = "↑";
        this.stockCodeDirection = "－";
        this.typeDirection = "－";
        this.priceDirection = "－";
        this.volumeDirection = "－";
        this.feeDirection = "－";
        this.lendingPerioDirection = "－";
        this.returnDateDirection = "－";
        this.isButtonDisabled = false;
        var currentDate = new Date();
        var year = currentDate.getFullYear();
        var month = ('0' + (currentDate.getMonth() + 1)).slice(-2);
        var day = ('0' + currentDate.getDate()).slice(-2);
        this.endDate = year + "-" + month + "-" + day;
    }
    return DataModel;
}());
exports.DataModel = DataModel;
//# sourceMappingURL=data-model.js.map