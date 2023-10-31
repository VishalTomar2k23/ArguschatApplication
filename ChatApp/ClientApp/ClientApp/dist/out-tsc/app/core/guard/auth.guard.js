"use strict";
exports.__esModule = true;
exports.authGuard = void 0;
var authGuard = function (route, state) {
    if (localStorage.getItem('isLoggedin')) {
        // logged in so return true
        return true;
    }
    // not logged in so redirect to login page with the return url
    // uncomment below line
    // this.router.navigate(['/auth/login'], { queryParams: { returnUrl: state.url } });
    return false;
};
exports.authGuard = authGuard;
//# sourceMappingURL=auth.guard.js.map