"use strict";
exports.__esModule = true;
var platform_browser_1 = require("@angular/platform-browser");
var app_component_1 = require("./app/app.component");
var http_1 = require("@angular/common/http");
platform_browser_1.bootstrapApplication(app_component_1.AppComponent, {
    providers: [http_1.provideHttpClient()]
})["catch"](function (err) { return console.error(err); });
//# sourceMappingURL=main.js.map