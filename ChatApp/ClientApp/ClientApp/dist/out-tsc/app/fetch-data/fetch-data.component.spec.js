"use strict";
exports.__esModule = true;
var testing_1 = require("@angular/core/testing");
var fetch_data_component_1 = require("./fetch-data.component");
describe('FetchDataComponent', function () {
    var component;
    var fixture;
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            imports: [fetch_data_component_1.FetchDataComponent]
        });
        fixture = testing_1.TestBed.createComponent(fetch_data_component_1.FetchDataComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });
    it('should create', function () {
        expect(component).toBeTruthy();
    });
});
//# sourceMappingURL=fetch-data.component.spec.js.map