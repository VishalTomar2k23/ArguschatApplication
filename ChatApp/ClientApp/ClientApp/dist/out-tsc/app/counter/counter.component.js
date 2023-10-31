"use strict";
exports.__esModule = true;
exports.CounterComponent = void 0;
var tslib_1 = require("tslib");
var core_1 = require("@angular/core");
var common_1 = require("@angular/common");
var CounterComponent = /** @class */ (function () {
    function CounterComponent() {
        this.currentCount = 0;
    }
    CounterComponent.prototype.incrementCounter = function () {
        this.currentCount++;
    };
    CounterComponent = tslib_1.__decorate([
        core_1.Component({
            selector: 'app-counter',
            standalone: true,
            imports: [common_1.CommonModule],
            templateUrl: './counter.component.html',
            styleUrls: ['./counter.component.css']
        })
    ], CounterComponent);
    return CounterComponent;
}());
exports.CounterComponent = CounterComponent;
//# sourceMappingURL=counter.component.js.map