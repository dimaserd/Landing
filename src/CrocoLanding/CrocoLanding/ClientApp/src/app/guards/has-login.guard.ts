import { map } from 'rxjs/operators';
import { LoginService } from 'src/services/LoginService';
import { Observable } from 'rxjs';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class HasLoginGuard implements CanActivate {
  constructor(private loginService: LoginService, private router: Router){}
  canActivate(): Observable<boolean | UrlTree> {
    return this.loginService.getLoginDataHelfly().pipe(map(({isAuthenticated})=>{
        return isAuthenticated ? true : this.router.parseUrl('/login');
    }));
  }
}
