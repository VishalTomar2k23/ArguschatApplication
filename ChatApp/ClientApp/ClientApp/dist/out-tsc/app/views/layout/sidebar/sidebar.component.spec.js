"use strict";
exports.__esModule = true;
var testing_1 = require("@angular/core/testing");
var sidebar_component_1 = require("./sidebar.component");
describe('SidebarComponent', function () {
    var component;
    var fixture;
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            imports: [sidebar_component_1.SidebarComponent]
        });
        fixture = testing_1.TestBed.createComponent(sidebar_component_1.SidebarComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });
    it('should create', function () {
        expect(component).toBeTruthy();
    });
});
//# sourceMappingURL=sidebar.component.spec.js.map