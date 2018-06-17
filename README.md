# ASP.NET MVC5 With Angular 2+

How to add Angular 2+ (NOT angular.js so) to an existing ASP.NET MVC 5 (NOT .Net core so) project for having an hybrid front-end:

## run the POC

Software pre-requites (tested on win10 - x64)
- Microsoft Visual Studio 2017 (Community Edition minimum)
- [Install the Latest NodeJs LTS with NPM](https://nodejs.org/en/)
- TypeScript 2.6 minimum.

Installation:
- Get the repo
- Open the solution -> manage NuGet packages -> restore
- Navigate with command prompt to the AngularApp folder into the project -> npm install
- after npm install from the same folder in command prompt: ng build --watch
- In solution -> start debbuging

### stack

.NET
- .NET Framework 4.6.1
- ASP.NET MVC 5.2.4
- Razor (works with forms as well)
- Forms authentication (just an example, even better with openId)
- ASP.NET Identity (just an example)

Angular
- Angular CLI: 6.0.7
- Node: 8.11.2

## How to reproduce

We considere the .Net project is an existing one. Actually for the POC, it's a new MVC Project with forms authentication .

pre-requites: Angular cli
- [Have a recent node.js & NPM (at this time, LTS in Node 8+)](https://nodejs.org/en/download/)
- [Angular CLI (at this time, V 6)](https://github.com/angular/angular-cli/wiki)

### Create a new Angular project

Navigate with command prompt to your MVC WEB project root path and run
```
	ng new AngularApp
```

Or whatever name you want. The created folder will be included into your project, just think about that.

Add the essentials following files to your project:
- AngularApp/Src folder
- AngularApp/package.json
- AngularApp/angular.json
- AngularApp/tsconfig.json
- AngularApp/node_modules

Update configuration settings:

- angular.json: change the outputPath to make it pointing to ../Scripts/angular folder as part of your MVC 5 project

````json
	...,
	"build": {
          "builder": "@angular-devkit/build-angular:browser",
          "options": {
            "outputPath": "../Scripts/angular",
			...
````

 If you want to use the same ASP.NET layout as your MVC app, add <base href="/"> on its header.
 It could be done in angular by programming too but be carreful, it will be new variable to set for deployment.

 Create a new view in your project. This view will be the host of the angular app. 
 In my POC, just another view named Angular linked to the Home controller with the same layout as the web app.

 Add the javascript links for the angular js files (another constrain to manage for deployment: theses files names) 
 and the app-root tag (It tells Angular where our angular components should be rendered)

 [Complete HTML of the Angular.cshtml view](./Views/Home/Angular.cshtml)

Add an action to your MVC controler for this view, a link to the view in your layout for example and that's it!

### Navigation

How to navigate from your .Net part to your angular part? It's really not complicate but think about one thing: 
everytime you want to display a .Net page with angular on it, you bootstrap the angular root module to launch the application. 
It means angular will start everytime you are displaying a page in .Net with the angular js files on it.

As a simple example, we will create a navigation in angular to show how the hybrid works (page1, page2, page3 and nav)

Create the 4 angular components: Navigate with command prompt to your Angular root folder path (AngularApp for me) and run
````
  ng g component page1
  ng g component page2
  ng g component page3
  ng g component nav
````
Your new components are created in AngularApp/src/app

In AngularApp/src, create the new file app.routes.ts
[app.routes.ts](./AngularApp/src/app/app.routes.ts)

In this POC, my .Net manage /Home/Angular to display my MVC view which will bootstrap Angular. 
All the other asks after /Home/Angular/ (e.g. /Home/Angular/link1, /Home/Angular/etc/1) 
will be managed by angular because the Angular router is loaded from it.

In AngularApp/src/app.module.ts, Import the RouterModule and your ROUTES file
[app.module.ts](./AngularApp/src/app/app.module.ts)

````typescript
import { RouterModule } from '@angular/router';
import { ROUTES } from './app.routes';

@NgModule({
  ...,
  imports: [
    ...,
    RouterModule.forRoot(ROUTES)
  ],
  ...
})
````

Change the AngularApp/src/app/nav/nav.component.html HTML content
[nav.component.html](./AngularApp/src/app/nav/nav.component.html)

Note:
- routerLink: navigate to Angular page
- HREF: navigate to .Net page

if HREF point to an .Net managed URL where ANgular is present, Angular will load and take the url for itself

You can test and and see you can navigate from .Net to Angular, Angular to .Net, navigate into Angular and even ask a refresh on the page where ANgular is present: it will manage the URL as well. Of course there is a boostrap and you must be aware about that.

TBC with
- data exchange in javascript (get the user profile for expl.)
- forms authentication (yeah, it works)
- Anti-forgery for protecting WEB API called by Angular


Thanks for the help:
- [MITHUN PATTANKAR](http://www.mithunvp.com/angular-asp-net-mvc-5-angular-cli-visual-studio-2017/)

