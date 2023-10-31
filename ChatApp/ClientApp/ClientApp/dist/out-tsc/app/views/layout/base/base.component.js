"use strict";
exports.__esModule = true;
exports.BaseComponent = void 0;
var tslib_1 = require("tslib");
var core_1 = require("@angular/core");
var common_1 = require("@angular/common");
var router_1 = require("@angular/router");
var footer_component_1 = require("../footer/footer.component");
var navbar_component_1 = require("../navbar/navbar.component");
var sidebar_component_1 = require("../sidebar/sidebar.component");
var BaseComponent = /** @class */ (function () {
    function BaseComponent(router) {
        var _this = this;
        this.router = router;
        // Spinner for lazyload modules
        router.events.forEach(function (event) {
            if (event instanceof router_1.RouteConfigLoadStart) {
                _this.isLoading = true;
            }
            else if (event instanceof router_1.RouteConfigLoadEnd) {
                _this.isLoading = false;
            }
        });
    }
    BaseComponent.prototype.ngOnInit = function () {
    };
    BaseComponent = tslib_1.__decorate([
        core_1.Component({
            selector: 'app-base',
            standalone: true,
            imports: [common_1.CommonModule, router_1.RouterModule, footer_component_1.FooterComponent, navbar_component_1.NavbarComponent, sidebar_component_1.SidebarComponent],
            templateUrl: './base.component.html',
            styleUrls: ['./base.component.css']
        })
    ], BaseComponent);
    return BaseComponent;
}());
exports.BaseComponent = BaseComponent;
//# sourceMappingURL=base.component.js.map