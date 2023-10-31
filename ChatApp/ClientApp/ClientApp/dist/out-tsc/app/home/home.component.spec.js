"use strict";
exports.__esModule = true;
var testing_1 = require("@angular/core/testing");
var home_component_1 = require("./home.component");
describe('HomeComponent', function () {
    var component;
    var fixture;
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            imports: [home_component_1.HomeComponent]
        });
        fixture = testing_1.TestBed.createComponent(home_component_1.HomeComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });
    it('should create', function () {
        expect(component).toBeTruthy();
    });
});
//# sourceMappingURL=home.component.spec.js.map