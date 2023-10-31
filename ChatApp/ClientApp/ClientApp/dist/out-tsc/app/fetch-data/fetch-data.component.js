"use strict";
exports.__esModule = true;
exports.FetchDataComponent = void 0;
var tslib_1 = require("tslib");
var core_1 = require("@angular/core");
var common_1 = require("@angular/common");
var http_1 = require("@angular/common/http");
var FetchDataComponent = /** @class */ (function () {
    function FetchDataComponent(http, baseUrl) {
        // http.get<WeatherForecast[]>(baseUrl + 'weatherforecast').subscribe(result => {
        //   this.forecasts = result;
        // }, error => console.error(error));
    }
    FetchDataComponent = tslib_1.__decorate([
        core_1.Component({
            selector: 'app-fetch-data',
            standalone: true,
            imports: [common_1.CommonModule, http_1.HttpClientModule],
            templateUrl: './fetch-data.component.html',
            styleUrls: ['./fetch-data.component.css']
        }),
        tslib_1.__param(1, core_1.Inject('BASE_URL'))
    ], FetchDataComponent);
    return FetchDataComponent;
}());
exports.FetchDataComponent = FetchDataComponent;
//# sourceMappingURL=fetch-data.component.js.map