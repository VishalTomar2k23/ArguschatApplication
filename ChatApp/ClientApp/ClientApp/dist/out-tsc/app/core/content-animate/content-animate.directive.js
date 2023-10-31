"use strict";
exports.__esModule = true;
exports.ContentAnimateDirective = void 0;
var tslib_1 = require("tslib");
var animations_1 = require("@angular/animations");
var core_1 = require("@angular/core");
var router_1 = require("@angular/router");
var ContentAnimateDirective = /** @class */ (function () {
    function ContentAnimateDirective(el, router, animationBuilder) {
        this.el = el;
        this.router = router;
        this.animationBuilder = animationBuilder;
    }
    ContentAnimateDirective.prototype.ngOnInit = function () {
        var _this = this;
        // animate the content
        this.initAnimate();
        // animate page load
        this.events = this.router.events.subscribe(function (event) {
            if (event instanceof router_1.NavigationEnd) {
                _this.player.play();
            }
        });
    };
    ContentAnimateDirective.prototype.ngOnDestroy = function () {
        this.events.unsubscribe();
        this.player.destroy();
    };
    ContentAnimateDirective.prototype.initAnimate = function () {
        this.player = this.animationBuilder
            .build([
            animations_1.style({ opacity: 0, transform: 'translateY(15px)' }),
            animations_1.animate(500, animations_1.style({ opacity: 1, transform: 'translateY(0)' })),
            animations_1.style({ transform: 'none' }),
        ])
            .create(this.el.nativeElement);
    };
    ContentAnimateDirective = tslib_1.__decorate([
        core_1.Directive({
            selector: '[appContentAnimate]',
            standalone: true
        })
    ], ContentAnimateDirective);
    return ContentAnimateDirective;
}());
exports.ContentAnimateDirective = ContentAnimateDirective;
//# sourceMappingURL=content-animate.directive.js.map