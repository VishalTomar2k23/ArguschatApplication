"use strict";
exports.__esModule = true;
exports.NavbarComponent = void 0;
var tslib_1 = require("tslib");
var core_1 = require("@angular/core");
var common_1 = require("@angular/common");
var NavbarComponent = /** @class */ (function () {
    function NavbarComponent(document, renderer, router) {
        this.document = document;
        this.renderer = renderer;
        this.router = router;
    }
    NavbarComponent.prototype.ngOnInit = function () {
    };
    /**
     * Sidebar toggle on hamburger button click
     */
    NavbarComponent.prototype.toggleSidebar = function (e) {
        e.preventDefault();
        this.document.body.classList.toggle('sidebar-open');
    };
    /**
     * Logout
     */
    NavbarComponent.prototype.onLogout = function (e) {
        e.preventDefault();
        localStorage.removeItem('isLoggedin');
        if (!localStorage.getItem('isLoggedin')) {
            this.router.navigate(['/auth/login']);
        }
    };
    NavbarComponent = tslib_1.__decorate([
        core_1.Component({
            selector: 'app-navbar',
            standalone: true,
            imports: [common_1.CommonModule],
            templateUrl: './navbar.component.html',
            styleUrls: ['./navbar.component.css']
        }),
        tslib_1.__param(0, core_1.Inject(common_1.DOCUMENT))
    ], NavbarComponent);
    return NavbarComponent;
}());
exports.NavbarComponent = NavbarComponent;
//# sourceMappingURL=navbar.component.js.map