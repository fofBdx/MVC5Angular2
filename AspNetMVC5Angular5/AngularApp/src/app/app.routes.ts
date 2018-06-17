import { Routes } from '@angular/router';
import { Page1Component} from './page1/page1.component';
import { Page2Component } from './page2/page2.component';
import { Page3Component } from './page3/page3.component';

export const ROUTES: Routes = [
  { path: 'Home/Angular', redirectTo: 'Home/Angular/page1' },
  { path: 'Home/Angular/page1', component: Page1Component },
  { path: 'Home/Angular/page2', component: Page2Component },
  { path: 'Home/Angular/page3', component: Page3Component }
];


