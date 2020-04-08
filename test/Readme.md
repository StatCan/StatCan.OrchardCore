# Cypress e2e testing suite

Cypress e2e testing suite for Innovation System Orchard

Useful commands

- `npm run host` hosts a version of OC running in production mode (faster)
- `npm run build` builds the application
- `npm run clean` deletes `/src/Inno.Web/App_Data` use with caution
- `npm run test` hosts the site then runs cypress tests
- `npm run test:clean` cleans `App_Data`, hosts the site, run cypress tests. Since tests require a clean slate, this is recommended
- `npm run test:clean-build` cleans `App_Data`, builds the site, hosts the site and runs cypress tests
- `npm run cypress` opens cypress dashboard
- `npm run cypress:run` runs the cypress tests. Assumes a pre-running instance.