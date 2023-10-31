"use strict";
exports.__esModule = true;
var tslib_1 = require("tslib");
var testing_1 = require("@angular/core/testing");
var auth_guard_1 = require("./auth.guard");
describe('authGuard', function () {
    var executeGuard = function () {
        var guardParameters = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            guardParameters[_i] = arguments[_i];
        }
        return testing_1.TestBed.runInInjectionContext(function () { return auth_guard_1.authGuard.apply(void 0, tslib_1.__spreadArray([], tslib_1.__read(guardParameters))); });
    };
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({});
    });
    it('should be created', function () {
        expect(executeGuard).toBeTruthy();
    });
});
//# sourceMappingURL=auth.guard.spec.js.map