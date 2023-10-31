import { AnimationPlayer , AnimationBuilder, style, animate } from '@angular/animations';
import { Directive, ElementRef, OnDestroy, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Directive({
  selector: '[appContentAnimate]',
  standalone: true
})
export class ContentAnimateDirective implements OnInit, OnDestroy {

  // Public properties
	player!: AnimationPlayer;
	// Private properties
	private events!: Subscription ;

	constructor(
		private el: ElementRef,
		private router: Router,
		private animationBuilder: AnimationBuilder) {
	}

	ngOnInit(): void {
		// animate the content
		this.initAnimate();
		// animate page load
		this.events = this.router.events.subscribe(event => {
			if (event instanceof NavigationEnd) {
				this.player.play();
			}
		});
	}

	ngOnDestroy(): void {
		this.events.unsubscribe();
		this.player.destroy();
	}
  
  initAnimate() {
		this.player = this.animationBuilder
			.build([
				style({opacity: 0, transform: 'translateY(15px)'}),
				animate(500, style({opacity: 1, transform: 'translateY(0)'})),
				style({transform: 'none'}),
			])
			.create(this.el.nativeElement);
	}
}
