"use strict";
exports.__esModule = true;
var testing_1 = require("@angular/core/testing");
var base_component_1 = require("./base.component");
describe('BaseComponent', function () {
    var component;
    var fixture;
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            imports: [base_component_1.BaseComponent]
        });
        fixture = testing_1.TestBed.createComponent(base_component_1.BaseComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });
    it('should create', function () {
        expect(component).toBeTruthy();
    });
});
//# sourceMappingURL=base.component.spec.js.map