"use strict";
exports.__esModule = true;
exports.SidebarComponent = void 0;
var tslib_1 = require("tslib");
var core_1 = require("@angular/core");
var common_1 = require("@angular/common");
var router_1 = require("@angular/router");
var menu_1 = require("./menu");
var metismenujs_1 = require("metismenujs");
var router_2 = require("@angular/router");
var SidebarComponent = /** @class */ (function () {
    function SidebarComponent(document, renderer, router) {
        var _this = this;
        this.document = document;
        this.renderer = renderer;
        this.menuItems = [];
        router.events.forEach(function (event) {
            if (event instanceof router_1.NavigationEnd) {
                /**
                 * Activating the current active item dropdown
                 */
                _this._activateMenuDropdown();
                /**
                 * closing the sidebar
                 */
                if (window.matchMedia('(max-width: 991px)').matches) {
                    _this.document.body.classList.remove('sidebar-open');
                }
            }
        });
    }
    SidebarComponent.prototype.ngOnInit = function () {
        this.menuItems = menu_1.MENU;
        /**
         * Sidebar-folded on desktop (min-width:992px and max-width: 1199px)
         */
        var desktopMedium = window.matchMedia('(min-width:992px) and (max-width: 1199px)');
        desktopMedium.addListener(this.iconSidebar);
        this.iconSidebar(desktopMedium);
    };
    SidebarComponent.prototype.ngAfterViewInit = function () {
        // activate menu item
        new metismenujs_1.MetisMenu(this.sidebarMenu.nativeElement);
        this._activateMenuDropdown();
    };
    /**
     * Toggle sidebar on hamburger button click
     */
    SidebarComponent.prototype.toggleSidebar = function (e) {
        this.sidebarToggler.nativeElement.classList.toggle('active');
        this.sidebarToggler.nativeElement.classList.toggle('not-active');
        if (window.matchMedia('(min-width: 992px)').matches) {
            e.preventDefault();
            this.document.body.classList.toggle('sidebar-folded');
        }
        else if (window.matchMedia('(max-width: 991px)').matches) {
            e.preventDefault();
            this.document.body.classList.toggle('sidebar-open');
        }
    };
    /**
     * Toggle settings-sidebar
     */
    SidebarComponent.prototype.toggleSettingsSidebar = function (e) {
        e.preventDefault();
        this.document.body.classList.toggle('settings-open');
    };
    /**
     * Open sidebar when hover (in folded folded state)
     */
    SidebarComponent.prototype.operSidebarFolded = function () {
        if (this.document.body.classList.contains('sidebar-folded')) {
            this.document.body.classList.add("open-sidebar-folded");
        }
    };
    /**
     * Fold sidebar after mouse leave (in folded state)
     */
    SidebarComponent.prototype.closeSidebarFolded = function () {
        if (this.document.body.classList.contains('sidebar-folded')) {
            this.document.body.classList.remove("open-sidebar-folded");
        }
    };
    /**
     * Sidebar-folded on desktop (min-width:992px and max-width: 1199px)
     */
    SidebarComponent.prototype.iconSidebar = function (e) {
        if (e.matches) {
            this.document.body.classList.add('sidebar-folded');
        }
        else {
            this.document.body.classList.remove('sidebar-folded');
        }
    };
    /**
     * Switching sidebar light/dark
     */
    SidebarComponent.prototype.onSidebarThemeChange = function (event) {
        this.document.body.classList.remove('sidebar-light', 'sidebar-dark');
        this.document.body.classList.add(event.target.value);
        this.document.body.classList.remove('settings-open');
    };
    /**
     * Returns true or false if given menu item has child or not
     * @param item menuItem
     */
    SidebarComponent.prototype.hasItems = function (item) {
        return item.subItems !== undefined ? item.subItems.length > 0 : false;
    };
    /**
     * Reset the menus then hilight current active menu item
     */
    SidebarComponent.prototype._activateMenuDropdown = function () {
        this.resetMenuItems();
        this.activateMenuItems();
    };
    /**
     * Resets the menus
     */
    SidebarComponent.prototype.resetMenuItems = function () {
        var links = document.getElementsByClassName('nav-link-ref');
        for (var i = 0; i < links.length; i++) {
            var menuItemEl = links[i];
            menuItemEl.classList.remove('mm-active');
            var parentEl = menuItemEl.parentElement;
            if (parentEl) {
                parentEl.classList.remove('mm-active');
                var parent2El = parentEl.parentElement;
                if (parent2El) {
                    parent2El.classList.remove('mm-show');
                }
                var parent3El = parent2El === null || parent2El === void 0 ? void 0 : parent2El.parentElement;
                if (parent3El) {
                    parent3El.classList.remove('mm-active');
                    if (parent3El.classList.contains('side-nav-item')) {
                        var firstAnchor = parent3El.querySelector('.side-nav-link-a-ref');
                        if (firstAnchor) {
                            firstAnchor.classList.remove('mm-active');
                        }
                    }
                    var parent4El = parent3El.parentElement;
                    if (parent4El) {
                        parent4El.classList.remove('mm-show');
                        var parent5El = parent4El.parentElement;
                        if (parent5El) {
                            parent5El.classList.remove('mm-active');
                        }
                    }
                }
            }
        }
    };
    ;
    /**
     * Toggles the menu items
     */
    SidebarComponent.prototype.activateMenuItems = function () {
        var links = document.getElementsByClassName('nav-link-ref');
        var menuItemEl = null;
        for (var i = 0; i < links.length; i++) {
            // tslint:disable-next-line: no-string-literal
            if (links[i].pathname === window.location.pathname) {
                menuItemEl = links[i];
                break;
            }
        }
        if (menuItemEl) {
            menuItemEl.classList.add('mm-active');
            var parentEl = menuItemEl.parentElement;
            if (parentEl) {
                parentEl.classList.add('mm-active');
                var parent2El = parentEl.parentElement;
                if (parent2El) {
                    parent2El.classList.add('mm-show');
                }
                var parent3El = parent2El === null || parent2El === void 0 ? void 0 : parent2El.parentElement;
                if (parent3El) {
                    parent3El.classList.add('mm-active');
                    if (parent3El.classList.contains('side-nav-item')) {
                        var firstAnchor = parent3El.querySelector('.side-nav-link-a-ref');
                        if (firstAnchor) {
                            firstAnchor.classList.add('mm-active');
                        }
                    }
                    var parent4El = parent3El.parentElement;
                    if (parent4El) {
                        parent4El.classList.add('mm-show');
                        var parent5El = parent4El.parentElement;
                        if (parent5El) {
                            parent5El.classList.add('mm-active');
                        }
                    }
                }
            }
        }
    };
    ;
    tslib_1.__decorate([
        core_1.ViewChild('sidebarToggler')
    ], SidebarComponent.prototype, "sidebarToggler");
    tslib_1.__decorate([
        core_1.ViewChild('sidebarMenu')
    ], SidebarComponent.prototype, "sidebarMenu");
    SidebarComponent = tslib_1.__decorate([
        core_1.Component({
            selector: 'app-sidebar',
            standalone: true,
            imports: [common_1.CommonModule, router_2.RouterModule],
            templateUrl: './sidebar.component.html',
            styleUrls: ['./sidebar.component.css']
        }),
        tslib_1.__param(0, core_1.Inject(common_1.DOCUMENT))
    ], SidebarComponent);
    return SidebarComponent;
}());
exports.SidebarComponent = SidebarComponent;
function activateMenuItems() {
    throw new Error('Function not implemented.');
}
//# sourceMappingURL=sidebar.component.js.map