import { ActivatedRouteSnapshot, CanActivateFn, RouterStateSnapshot } from '@angular/router';

export const authGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state:RouterStateSnapshot) => {
  if (localStorage.getItem('isLoggedin')) {
    // logged in so return true
    return true;
  }

  // not logged in so redirect to login page with the return url
  // uncomment below line
  // this.router.navigate(['/auth/login'], { queryParams: { returnUrl: state.url } });
  return false;  
};
