"use strict";
exports.__esModule = true;
exports.FeatherIconDirective = void 0;
var tslib_1 = require("tslib");
var core_1 = require("@angular/core");
var feather = require("feather-icons");
var FeatherIconDirective = /** @class */ (function () {
    function FeatherIconDirective() {
    }
    FeatherIconDirective.prototype.ngAfterViewInit = function () {
        // feather icon
        feather.replace();
    };
    FeatherIconDirective = tslib_1.__decorate([
        core_1.Directive({
            selector: '[appFeatherIcon]',
            standalone: true
        })
    ], FeatherIconDirective);
    return FeatherIconDirective;
}());
exports.FeatherIconDirective = FeatherIconDirective;
//# sourceMappingURL=feather-icon.directive.js.map