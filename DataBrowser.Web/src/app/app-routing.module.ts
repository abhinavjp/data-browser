import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './authentication/user-login/login.component';
import { RegistrationComponent } from './authentication/user-registration/registration.component';


const appRoutes: Routes = [

    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegistrationComponent },
    {
        path: '',
        redirectTo: '/login',
        pathMatch: 'full'
    },
    { path: '**', component: LoginComponent }
];

export const routing = RouterModule.forRoot(appRoutes);