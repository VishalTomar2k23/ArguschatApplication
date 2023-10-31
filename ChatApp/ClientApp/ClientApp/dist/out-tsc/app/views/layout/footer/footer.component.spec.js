"use strict";
exports.__esModule = true;
var testing_1 = require("@angular/core/testing");
var footer_component_1 = require("./footer.component");
describe('FooterComponent', function () {
    var component;
    var fixture;
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            imports: [footer_component_1.FooterComponent]
        });
        fixture = testing_1.TestBed.createComponent(footer_component_1.FooterComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });
    it('should create', function () {
        expect(component).toBeTruthy();
    });
});
//# sourceMappingURL=footer.component.spec.js.map