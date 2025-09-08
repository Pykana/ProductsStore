import { Routes } from '@angular/router';
import { Login } from './Components/login/login';

export const routes: Routes = [
    // {path: 'login', loadComponent: () => import('./Components/login/login').then(c => c.Login)},
    {path: '' , component: Login}
];
