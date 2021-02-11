# ClientApp

Orchard Skills, Inc. Client Application

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 9.0.1.

[Node.js](https://nodejs.org/en/docs/)

[Yarn](https://yarnpkg.com)

[Angular](https://angular.io/)

[AngularCLI](https://cli.angular.io/)

[Jest](https://jestjs.io)

[Angular Style Guide](https://angular.io/guide/styleguide)

[RxJS](https://rxjs-dev.firebaseapp.com/)

[Firebase](https://firebase.google.com/docs/)

[Firebase CLI](https://firebase.google.com/docs/cli/)

[AngularFire](https://github.com/angular/angularfire2)

## Common NPM commands in Yarn

|NPM Command                                                                                | Yarn Command              | Description (_wherever necessary_)                                                 |
|:------------------------------------------------------------------------------------------|:--------------------------|------------------------------------------------------------------------------------|
|npm install                                                                                |yarn <br/> yarn install    |Will install packages listed in the `package.json` file                             |
|npm install `pkg-name` <br/> npm install --save `pkg-name`                                 | yarn add `pkg-name`       |By default Yarn adds the `pgk-name` to `package.json` and `yarn.lock` files         |
|npm install `pkg-name@1.0.0`                                                               | yarn add `pgk-name@1.0.0` |                                                                                    |
|npm install `pkg-name` --save-dev                                                          | yarn add `pkg-name` --dev |                                                                                    |
|npm install `pkg-name` --peer                                                              | yarn add `pkg-name`--peer |                                                                                    |
|npm install `pkg-name` --optional                                                          | yarn add --optional       |                                                                                    |
|npm install -g `pkg-name`                                                                  | yarn global add `pkg-name`| Careful, yarn add global `pkg-name` adds packages `global` and `pkg-name` locally! |
|npm update                                                                                 | yarn upgrade              | Note: It's called **upgrade** in yarn                                              |
|npm uninstall `pkg-name`                                                                   | yarn remove `pkg-name`    |                                                                                    |
|npm run `script-name`                                                                      | yarn run `script-name`    |                                                                                    |
|npm init                                                                                   | yarn init                 |                                                                                    |
|npm pack                                                                                   | yarn pack                 | Creates a compressed gzip archive of the package dependencies                      |
|npm link                                                                                   | yarn link                 |                                                                                    |
|npm outdated                                                                               | yarn outdated             |                                                                                    |
|npm publish                                                                                | yarn publish              |                                                                                    |
|npm run                                                                                    | yarn run                  |                                                                                    |
|npm cache clean                                                                            | yarn cache clean          |                                                                                    |
|npm login                                                                                  | yarn login (and logout)   |                                                                                    |
|npm test                                                                                   | yarn test                 |                                                                                    |
|npm install --production                                                                   | yarn --production         |                                                                                    |
|npm  --version                                                                             | yarn version              |                                                                                    |
|npm  info | yarn info|


### New Commands in Yarn

|Yarn Command                      | Description                                                               |
|----------------------------------|---------------------------------------------------------------------------|
|yarn why `pkg-name`               | Builds a dependency graph on why this package is being used               |
|yarn clean                        | Frees up space by removing unnecessary files and folders from dependencies|
|yarn licenses ls                  | Inspect the licenses of your dependencies                                 |
|yarn licenses generate-disclaimer | Automatically create your license dependency disclaimer                   |

## Install the Angular CLI

### NPM

```
npm install -g @angular/cli
```

### Yarn

```
yarn global add @angular/cli
```

## Install TypeScript

### NPM

```
npm install -g typescript
```

### Yarn

```
yarn global add typescript
```

## Install NPM Check Updates

### NPM

```
npm install -g npm-check-updates
```

### Yarn

```
yarn global add npm-check-updates
```

## Code scaffolding

### ng new command switches used

#### --style=[css | scss | less | sass | styl]

The style option specifies what CSS preprocessor is used in building the project. the options are: css, scss, less, sass, styl.

#### --routing

The routing option generates a file app-routing.module.ts file.

#### --skip-install

This skip-install option disables the npm install after code generation.

#### --skip-git

When true, does not initialize a git repository.

#### --minimal=[true|false]

When true, creates a project without any testing frameworks. (Use for learning purposes only.)

#### --inlineTemplate=[true|false]

When true, includes styles inline in the component TS file. By default, an external styles file is created and referenced in the component TS file.

#### --inlineStyle=[true|false]

When true, includes styles inline in the component TS file. By default, an external styles file is created and referenced in the component TS file.

#### --skipTests=[true|false]

When true, does not generate "spec.ts" test files for the new project.

## ng new

```
ng new ClientApp --routing --style=scss --skip-install --skip-git --skipTests=true
```

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

```
ng generate component blog -m app
ng generate component blog-content -m app
ng generate component navbar -m app
```

## Project Folder Structure

### Feature Modules

Feature Modules with designated folders for components. It’s perfect for scaling and the logic is loosely coupled.

```
|-- modules
       |-- feature
           |-- components
           |    |-- feature
           |         |-- feature.component.ts|html|scss|spec
           |
           |-- feature-routing.module.ts
           |-- feature.module.ts
```

### The Core Module

The CoreModule takes on the role of the root AppModule , but is not the module which gets bootstrapped by Angular at run-time. The CoreModule should contain singleton services (which is usually the case), universal components and other features where there’s only once instance per application. To prevent re-importing the core module elsewhere, you should also add a guard for it in the core module constructor.

```
|-- core
       |-- [+] authentication
       |-- [+] footer
       |-- [+] guards
       |-- [+] http
       |-- [+] interceptors
       |-- [+] mocks
       |-- [+] services
       |-- [+] header
       |-- core.module.ts
       |-- ensureModuleLoadedOnceGuard.ts
       |-- logger.service.ts
```

#### authentication

The authentication folder simply handles the authentication-cycle of the user (from login to logout).

```
|-- authentication
     |-- authentication.service.ts|spec.ts
```

#### header, footer

The footer and header folders contains the global component-files, statically used across the entire application. These files will appear on every page in the application.

```
|-- header
     |-- header.component.ts|html|scss|spec.ts
|-- footer
     |-- footer.component.ts|html|scss|spec.ts
```

#### http

The http folder handles stuff like http calls from our application. I’ve also added a api.service.ts file to keep all http calls running through our application in one single place. The folder does otherwise contain folders for interacting with the different API-routes.

```
|-- http
     |-- user
          |-- user.service.ts|spec.ts
     |-- api.service.ts|spec.ts
```

#### interceptors

Angular introduced a long-awaited feature for handling http requests — the HttpInterceptor interface. This allows us to catch and modify the requests and responses from our API calls. The interceptors folder is a collection of interceptors I find specially useful.

```
|-- interceptors
       |-- api-prefix.interceptor.ts
       |-- error-handler.interceptor.ts
       |-- http.token.interceptor.ts
```

#### guards

The guards folder contains all of the guards I use to protect different routes in my applications.

```
|-- guards
     |-- auth.guard.ts
     |-- no-auth-guard.ts
     |-- admin-guard.ts 
```

#### mocks

Mocks are specially useful for testing, but you can also use them to retrieve fictional data until the back-end is set up. The mocks folder contains all the mock-files of our app.

```
|-- mocks
       |-- user.mock.ts
```

#### services

All additional singleton services are placed in the services folder.

```
|-- services
     |-- srv1.service.ts|spec.ts
     |-- srv2.service.ts|spec.ts
```

### The Shared Module

The SharedModule is where any shared components, pipes/filters and services should go. The SharedModule can be imported in any other module when those items will be re-used. The shared module shouldn’t have any dependency to the rest of the application and should therefore not rely on any other module.

```
|-- shared
     |-- [+] components
     |-- [+] directives
     |-- [+] pipes
```

#### components

The components folder contains all the “shared” components. This are components like loaders and buttons , which multiple components would benefit from.

```
|-- components
     |-- loader
          |-- loader.component.ts|html|scss|spec.ts
     |-- buttons
          |-- favorite-button
               |-- favorite-button.component.ts|html|scss|spec.ts
          |-- collapse-button
               |-- collapse-button.component.ts|html|scss|spec.ts
```

#### directives, pipes, models

The directives , pipes and models folders contains the directives, pipes and models used across the application.

```
|-- directives
      |-- auth.directive.ts|spec.ts
|-- pipes
     |-- capitalize.pipe.ts
     |-- safe.pipe.ts
|-- models
     |-- user.model.ts
     |-- server-response.ts
```

#### Configurations

The config folder contains app settings and other predefined values.

```
|-- configurations
     |-- app-settings.config.ts
```

### Styling

#### The 7-1 Pattern

[The 7-1 Pattern](https://sass-guidelin.es/#the-7-1-pattern)

The global styles for the project are placed in a scss folder under assets .

```
|-- scss
     |-- partials
          |-- _layout.vars.scss
          |-- _responsive.partial.scss
     |-- _base.scss
|-- styles.scss
```

The scss folder does only contain one folder — partials. Partial-files can be imported by other scss files. In my case, styles.scss imports all the partials to apply their styling.

## Add Chrome/Firefox debugging configuration

Create folder .vscode and add file launch.json

```
{
    // Use IntelliSense to learn about possible attributes.
    // Hover to view descriptions of existing attributes.
    // For more information, visit: https://go.microsoft.com/fwlink/?linkid=830387
    "version": "0.2.0",
    "configurations": [
        {
            "name": "Launch Chrome against localhost:4200, with sourcemaps",
            "type": "chrome",
            "request": "launch",
            "url": "http://localhost:4200",
            "runtimeArgs": [
              "--new-window",
              "--incognito",
              "--remote-debugging-port=9222"
            ],
            "sourceMaps": true,
            "trace": "verbose",
            "webRoot": "${workspaceRoot}"
        },
        {
            "name": "Attach Chrome against, with sourcemaps",
            "type": "chrome",
            "request": "attach",
            "port": 9222,
            "sourceMaps": true,
            "trace": "verbose",
            "webRoot": "${workspaceRoot}"
        },
        {
            "name": "Launch Firefox against localhost:4200, with sourcemaps",
            "type": "firefox",
            "request": "launch",
            "reAttach": true,
            "url": "http://localhost:4200",
            "webRoot": "${workspaceFolder}"
        }
    ]
  }
```

## Enable TypeScript strict

Modify tsconfig.json add "strict": true,

```
"compilerOptions": {
    ...
    "target": "es2015",
    "strict": true,
    ...
}
```

## Modify .gitignore

Add the following to the .gitignore file

```
# node
package-lock.json

# yarn
yarn.lock
yarn-error.log

# Firebase
.firebase/*
firebase-debug.log
```

## Install the Jest Testing Framework

### Yarn

```
yarn add jest jest-preset-angular @types/jest --dev
```

###  NPM

``
npm install --save-dev jest jest-preset-angular @types/jest
``

### Create setupJest.ts

Create the setupJest.ts file in the root folder.

```
import 'jest';
import 'jest-preset-angular';
```

### Modify project.json

```
{
  ...
  "version": "1.0.0",
  "scripts": {
    "ng": "ng",
    "start": "ng serve",
    "build": "ng build",
    "test": "jest",
    "test:cc": "jest --coverage",
    "lint": "ng lint",
  },
  "private": true,
  "jest": {
    "preset": "jest-preset-angular",
    "setupFilesAfterEnv": [
      "<rootDir>/setupJest.ts"
    ],
     "testPathIgnorePatterns": [
      "<rootDir>/node_modules/",
      "<rootDir>/dist/",
      "<rootDir>/src/test.ts",
      "<rootDir>/cypress/",
      "<rootDir>/src/environments/"
    ],
    "globals": {
      "ts-jest": {
        "tsConfig": "<rootDir>/tsconfig.spec.json",
        "stringifyContentPathRegex": "\\.html$",
        "astTransformers": [
          "jest-preset-angular/build/InlineFilesTransformer",
          "jest-preset-angular/build/StripStylesTransformer"
        ]  
      }
    }
  },
  ...
}  
```

### Create the file tsconfig.spec.json file. Add the following configuration.

```
{
    "extends": "./tsconfig.json",
    "compilerOptions": {
      "emitDecoratorMetadata": true,
      "outDir": "./out-tsc/spec",
      "types": [
        "jest",
        "node"
      ]
    },
    "files": [
      "src/polyfills.ts"
    ],
    "include": [
      "src/**/*.spec.ts",
      "src/**/*.d.ts"
    ]
}
```

## Install Cypress End-to-End Testing framework

### Install the Cypress package

### Yarn

```
yarn add cypress --dev
```

### NPM

```
npm install --save-dev cypress
```

### Create Cypress Open Command

Replace the e2e with a cypress open command in the scripts section of our package.json

```
"cypress:open": "./node_modules/.bin/cypress open"
```

### Add Cypress folder to testPathIgnorePatterns in package.json

```
    "testPathIgnorePatterns": [
      "<rootDir>/node_modules/",
      "<rootDir>/dist/",
      "<rootDir>/src/test.ts",
      "<rootDir>/cypress/"
    ],
```

### Modify tsconfig.json

Add "types": ["cypress"], to the tsconfig.json file.

```
{
  "compileOnSave": false,
  "compilerOptions": {
    ...
    "target": "es2015",
    "types": ["cypress"],
  }
  ...
}
```

## Run Cypress

### Yarn

```
yarn cypress:open
```

### NPM

```
npm run cypress:open
```

After Cypress first run, it will create a cypress folder in the root directory. Move the examples folder from cypress/integration to cypress folder.

Create the file initial-page.spec.ts in the cypress/integration folder and add the following:

```
describe('initial-page', () => {

  beforeEach(() => {
    cy.visit("localhost:4200/");
  })

  it(`has title 'javascript-advisors'`, () => {
    cy.contains('javascript-advisors');
    cy.get('h1').should('contain', 'javascript-advisors');
    cy.title().should('eq', 'JavascriptAdvisors');
  })

})
```

## Create app.component files

Create the file app.component.html in the src/app folder and add the following code:

```
<div style="text-align:center" class="content">
    <h1>
          Welcome to {{title}}!
    </h1>
    <span style="display: block">{{ title }} app is running!</span>
    <img width="300" alt="Angular Logo" src="data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZpZXdCb3g9IjAgMCAyNTAgMjUwIj4KICAgIDxwYXRoIGZpbGw9IiNERDAwMzEiIGQ9Ik0xMjUgMzBMMzEuOSA2My4ybDE0LjIgMTIzLjFMMTI1IDIzMGw3OC45LTQzLjcgMTQuMi0xMjMuMXoiIC8+CiAgICA8cGF0aCBmaWxsPSIjQzMwMDJGIiBkPSJNMTI1IDMwdjIyLjItLjFWMjMwbDc4LjktNDMuNyAxNC4yLTEyMy4xTDEyNSAzMHoiIC8+CiAgICA8cGF0aCAgZmlsbD0iI0ZGRkZGRiIgZD0iTTEyNSA1Mi4xTDY2LjggMTgyLjZoMjEuN2wxMS43LTI5LjJoNDkuNGwxMS43IDI5LjJIMTgzTDEyNSA1Mi4xem0xNyA4My4zaC0zNGwxNy00MC45IDE3IDQwLjl6IiAvPgogIDwvc3ZnPg==">
</div>
<router-outlet></router-outlet>
```

Create a empty file app.component.scss in the src/app folder

Create the file app.component.spec.ts in the src/app folder and add the following:

```
import { TestBed, async } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AppComponent } from './app.component';

describe('AppComponent', () => {
    beforeEach(async(() => {
      TestBed.configureTestingModule({
        imports: [
          RouterTestingModule
        ],
        declarations: [
          AppComponent
        ],
      }).compileComponents();
    }));
  
    it('should create the app', () => {
      const fixture = TestBed.createComponent(AppComponent);
      const app = fixture.debugElement.componentInstance;
      expect(app).toBeTruthy();
    });
  
    it(`should have as title 'angular-advisors'`, () => {
        const fixture = TestBed.createComponent(AppComponent);
        const app = fixture.debugElement.componentInstance;
        expect(app.title).toEqual('javascript-advisors');
      });

    it('should render title', () => {
      const fixture = TestBed.createComponent(AppComponent);
      fixture.detectChanges();
      const compiled = fixture.debugElement.nativeElement;
      expect(compiled.querySelector('.content span').textContent).toContain('angular-advisors app is running!');
    });
  });  
```

Modify the file app.component.ts in the src/app folder. Change the @Component to:

```
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

```

## install Bulma (if using original Bulma)

### NPM

```
npm install bulma --save
```

### yarn

```
yarn add bulma
```

## Install BulmaSwatch (if using BulmaSwatch)

### NPM

```
npm install bulmaswatch --save
```

### yarn

```
yarn add bulmaswatch
```

## Install bulma-toast

### NPM

```
  npm install bulma-toast  --save
```

### yarn 

```
  yarn add bulma-toast
```

## Install WOW Animations

### NPM

```
npm install wowjs --save
```

### yarn

```
yarn add wowjs
```

## Install ngx-wow

### NPM

```
npm install ngx-wow --save 
```

### Yarn

```
yarn add ngx-wow
```

Modify angular.json and add the following

```
      "styles": [
        "../node_modules/animate.css/animate.css"
      ],
      "scripts": [
        "../node_modules/wowjs/dist/wow.js"
      ],
```

## Install ngx-wow

### NPM

```
npm install ngx-wow --save
```

### yarn

```
yarn add ngx-wow
```

## Install AOS  Animation

```
yarn add aos@next

or

npm install --save aos@next

```

### Import aos.js in angular.json 

```
  "scripts": [
      ....              
      "./node_modules/aos/dist/aos.js"              
        ]
```
### Import aos styles in style.scss

```
@import 'aos/dist/aos.css';
```

### Add in tsconfig.json

```
"compilerOptions": {
    ...
    "allowSyntheticDefaultImports": true,    
    ...
  }, 
```

### Add aos animations global in app.component.ts

```
import { Component, OnInit } from '@angular/core';
import AOS from 'aos';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  constructor() { }
  ngOnInit() {
    AOS.init({
      // Settings that can be overridden on per-element basis, by `data-aos-*` attributes:
      offset: 120, // offset (in px) from the original trigger point
      delay: 0, // values from 0 to 3000, with step 50ms
      duration: 1000, // values from 0 to 3000, with step 50ms
      easing: 'ease-in-out', // default easing for AOS animations
      once: false, // whether animation should happen only once - while scrolling down
    });
  }
}

```

### Use aos animation in any HTML component
```
  <div
    data-aos="fade-up"
    data-aos-offset="200"
    data-aos-delay="50"
    data-aos-duration="1000"
    data-aos-easing="ease-in-out"
    data-aos-mirror="true"
    data-aos-once="false"
    data-aos-anchor-placement="top-center"
  >
  </div>
```


## Add Icons Moon icomoon

### Link for icons type
```
http://www.vanessasalvador.com.br/e-commerce/plugins/themefisher-font/demo.html

or 

https://icomoon.io/
```



### Download icomoon font from 

```
https://drive.google.com/open?id=18RyXw813Yo7JzlALHcDkQpWDaKFQeNm0
```
### Add icomoon to the project

copy font folder that you download to project assets folder

```
assets/
   fonts/
```
### call the icon style.css from fonts folder in index.html

```
<head>
  ...
  <link href="./assets/fonts/style.css" rel="stylesheet">
  ...
</head>
```

### Use it in html

```
<i class="tf-edit"></i>
```
## Install packages

### NPM

```
npm install
```

### yarn

```
yarn install
```

## Check packages.json for outdated packages

At times, the package.json file can get out of date from what is current. To check for outdated packages install npm-check-updates, run ncu to see outdated packages, and then run ncu -u to update the packages.

### NPM

```
ncu
ncu -u
npm install
```

### YARN

```
ncu
ncu -u
yarn install
```

After the update, usually Angular does not support the latest version of TypeScript; therefore it must be set to the version supported by the Angular team witch is currently version: "typescript": "~3.5.3"

## Install fontawsome

Install the packages needed for Font Awesome.

### npm

```
npm install @fortawesome/free-brands-svg-icons --save
npm install @fortawesome/fontawesome-svg-core --save
npm install @fortawesome/free-solid-svg-icons --save
npm install @fortawesome/angular-fontawesome --save
```

## Yarn

```
yarn add @fortawesome/free-brands-svg-icons
yarn add @fortawesome/fontawesome-svg-core
yarn add @fortawesome/free-solid-svg-icons
yarn add @fortawesome/angular-fontawesome
```

## Install ngx-markdown

### NPM

```
npm install ngx-markdown --save
```

### Yarn

```
yarn add ngx-markdown
```

### angular.json

Modify angular.json and styles and scripts

```
            "styles": [
              "node_modules/prismjs/themes/prism-okaidia.css",
              "node_modules/prismjs/plugins/line-numbers/prism-line-numbers.css",
              "node_modules/prismjs/plugins/line-highlight/prism-line-highlight.css"
            ]
```

```
            "scripts": [
              "node_modules/marked/lib/marked.js",
              "node_modules/prismjs/prism.js",
              "node_modules/prismjs/components/prism-csharp.min.js",
              "node_modules/prismjs/components/prism-css.min.js",
              "node_modules/prismjs/plugins/line-numbers/prism-line-numbers.js",
              "node_modules/prismjs/plugins/line-highlight/prism-line-highlight.js"
            ]
```

## Install ngx-pagination

### NPM

```
npm install ngx-pagination --save
```

### Yarn

```
yarn add ngx-pagination
```
## Add GraphQL to project

### install Apollo GraphQL with Angular Schematics

```
ng add apollo-angular
```

### One thing you need to set is the URL of your GraphQL Server, so open 

```
src/app/graphql.module.ts
```
set the url variables

```
import {NgModule} from '@angular/core';
import {ApolloModule, APOLLO_OPTIONS} from 'apollo-angular';
import {HttpLinkModule, HttpLink} from 'apollo-angular-link-http';
import {InMemoryCache} from 'apollo-cache-inmemory';

const uri = 'https://orchardheadless.com/api/graphql'; // <-- add the URL of the GraphQL server here
export function createApollo(httpLink: HttpLink) {
  return {
    link: httpLink.create({uri}),
    cache: new InMemoryCache(),
  };
}

@NgModule({
  exports: [ApolloModule, HttpLinkModule],
  providers: [
    {
      provide: APOLLO_OPTIONS,
      useFactory: createApollo,
      deps: [HttpLink],
    },
  ],
})
export class GraphQLModule {}
```
## Install GraphQL Code Generator 

### NPM

```
npm install --save-dev graphql @graphql-codegen/cli
```

### Yarn

```
yarn add -D graphql @graphql-codegen/cli
```

### GraphQL Code Generator lets you setup everything by simply running the following command:
```
yarn run graphql-codegen init
```
Question by question, it will guide you through the whole process of setting up a schema, selecting plugins, picking a destination of a generated file and a lot more.

If you don't want to use the wizard, create a basic codegen.yml configuration file, point to your schema, and pick the plugins you wish to use. For example:

```
overwrite: true
schema: "https://orchardheadless.com/api/graphql"
documents: "src/app/graphql/*.graphql"
generates:
  src/app/graphql/graphql.ts:
    plugins:
      - "typescript"
      - "typescript-operations"
      - "typescript-apollo-angular"
  ./graphql.schema.json:
    plugins:
      - "introspection"

```

### Create GraphQL querys

Create folder graphql and inside of the folder crearte GraphQL querys files

```
posts.graphql
```

Inside of posts.graphql create a post query

```
 query BlogPosts {
    blogPost {
        path
        subtitle
        displayText
        owner
        publishedUtc
        image {
          urls
        }
        markdownBody {
          markdown
        }
  }
}
```

Then, run the code-generator using graphql-codegen that you set in package.json command:

```
yarn run graphql-codegen
```
The command above will fetch the GraphQL schema and create graphql.ts


## Lets use graphql.ts in components

inside of blog.components.ts
```
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map, take} from 'rxjs/operators';
import { BlogPostsGQL, BlogPostsQuery, SearchQueryQuery, SearchQueryGQL, SearchQueryQueryVariables } from '../../../graphql/graphql';

@Component({
  selector: 'app-blog',
  templateUrl: './blog.component.html',
  styleUrls: ['./blog.component.scss']
})
export class BlogComponent implements OnInit {

 blogPosts!: Observable<BlogPostsQuery>; => Create blogPosts observable and make it as a type of BlogPostsQuery and import it from graphql.ts

  constructor(
    private allBlogPostGQL: BlogPostsGQL => inject BlogPostsGQL from graphql.ts
  ) {
  }

  ngOnInit() {
    this.blogPosts =  this.allBlogPostGQL.watch().valueChanges.pipe(map(blogs =>    blogs.data)) => call the query and fetch the data
  }
}
```

Inside of blog.component.html use ngFor to display the data

```
<ng-container *ngFor="let blog of (blogPosts | async)?.blogPost  | paginate: config">
          <div (click)="showMore(blog.path)" class="card block">
            <div class="card-image">
              <figure style="cursor: pointer;" (click)="showMore(blog.path)" class="image">

                <img class="image" src="https://www.orchardheadless.com/{{blog.mainBlogImage.urls}}"
                  alt="Placeholder image">

              </figure>
            </div>
            <div class="card-content">
              <p style="cursor: pointer;" (click)="showMore(blog.path)"
                class="title is-4 has-text-centered has-text-info">{{blog.displayText}}</p>
              <p style="cursor: pointer;" (click)="showMore(blog.path)"
                class="subtitle is-6 has-text-centered has-text-white">{{blog.subtitle}}
              </p>
              <p class="has-text-info">Posted by <span class="has-text-white">{{blog.owner}}</span> on <time
                  datetime="2016-1-1">{{blog.publishedUtc  | date}}</time></p>
              <hr>
              <div (click)="showMore(blog.path)" style="cursor: pointer;"
                class="content has-text-centered light-grey-text">

                <button class="button is-link is-rounded">Click here for more info</button>
                <br>

              </div>
            </div>

          </div>
        </ng-container>
```
## .gitignore

Add the following to the .gitignore file.

```
# node
package-lock.json

# yarn
yarn.lock
yarn-error.log

# Firebase
.firebase/*
firebase-debug.log
```

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).
