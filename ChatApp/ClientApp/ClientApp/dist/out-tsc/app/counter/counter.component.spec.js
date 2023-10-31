"use strict";
exports.__esModule = true;
var testing_1 = require("@angular/core/testing");
var counter_component_1 = require("./counter.component");
describe('CounterComponent', function () {
    var component;
    var fixture;
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            imports: [counter_component_1.CounterComponent]
        });
        fixture = testing_1.TestBed.createComponent(counter_component_1.CounterComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });
    it('should create', function () {
        expect(component).toBeTruthy();
    });
});
//# sourceMappingURL=counter.component.spec.js.map