"use strict";
exports.__esModule = true;
var testing_1 = require("@angular/core/testing");
var nav_menu_component_1 = require("./nav-menu.component");
describe('NavMenuComponent', function () {
    var component;
    var fixture;
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            imports: [nav_menu_component_1.NavMenuComponent]
        });
        fixture = testing_1.TestBed.createComponent(nav_menu_component_1.NavMenuComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });
    it('should create', function () {
        expect(component).toBeTruthy();
    });
});
//# sourceMappingURL=nav-menu.component.spec.js.map