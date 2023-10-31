"use strict";
exports.__esModule = true;
var testing_1 = require("@angular/core/testing");
var navbar_component_1 = require("./navbar.component");
describe('NavbarComponent', function () {
    var component;
    var fixture;
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            imports: [navbar_component_1.NavbarComponent]
        });
        fixture = testing_1.TestBed.createComponent(navbar_component_1.NavbarComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });
    it('should create', function () {
        expect(component).toBeTruthy();
    });
});
//# sourceMappingURL=navbar.component.spec.js.map