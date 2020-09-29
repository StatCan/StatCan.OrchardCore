# Testing

We use the Cypress JS testing framework to run E2E tests for this application.

## Useful commands

From the `/test` folder in the solution, you can run the following commands to simplify testing the application.

- `npm run host` hosts a version of OC running in production mode (faster).
- `npm run build` builds the application.
- `npm run clean` Deletes `/src/StatCan.OrchardCore.Cms.Web/App_Data`.
- `npm run test` hosts the site then runs cypress tests.
- `npm run test:clean` **Deletes `/src/StatCan.OrchardCore.Cms.Web/App_Data`**, hosts the site, run cypress tests.
- `npm run test:clean-build` cleans `App_Data`, builds the site, hosts the site and runs cypress tests. This is recommended.
- `npm run cypress` opens cypress dashboard.
- `npm run cypress:run` runs the cypress tests. Assumes a pre-running instance.