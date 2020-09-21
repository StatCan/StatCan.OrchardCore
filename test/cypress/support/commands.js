import { creds } from "./objects";

// ***********************************************
// https://on.cypress.io/custom-commands
// ***********************************************
Cypress.Commands.add("login", function({ prefix = "" } = {}) {
  cy.visit(`${prefix}/login`);
  cy.get("#UserName").type(creds.username);
  cy.get("#Password").type(creds.password);
  cy.get("form").submit();
});

Cypress.Commands.add("gotoTenantSetup", ({ name }) => {
  cy.visit("/Admin/Tenants");
  cy.get(`#btn-setup-${name}`).click();
});
Cypress.Commands.add(
  "setupSite",
  ({ name, setupRecipe, username, email, password, passwordConfirmation }) => {
    cy.get("#SiteName").type(name);
    cy.get("body").then($body => {
      const elem = $body.find("#RecipeName");
      if (elem) {
        elem.val(setupRecipe);
      }
      const db = $body.find("#DatabaseProvider");
      if (db) {
        db.val("Sqlite");
      }
    });
    cy.get("#UserName").type(username);
    cy.get("#Email").type(email);
    cy.get("#Password").type(password);
    cy.get("#PasswordConfirmation").type(passwordConfirmation);
    cy.get("#SubmitButton").click();
  }
);
Cypress.Commands.add("createTenant", ({ name, prefix }) => {
  // We create tenants on the SaaS tenant
  cy.visit("/Admin/Tenants");
  // weak selector. CreateTenant
  cy.get('form > .row > .form-group > .btn-group > .btn').click()
  cy.get("#Name").type(name);
  cy.get("#RequestUrlPrefix").type(prefix);

  cy.get("body").then($body => {
    const elem = $body.find("#TablePrefix");
    if (elem && elem.is(":visible")) {
      elem.val(prefix);
    }
  });
  // submit button -- weak selector
  cy.get(".btn-primary").click();
});

Cypress.Commands.add("runRecipe", ({ prefix }, filterValue) => {
  cy.visit(`${prefix}/Admin/Recipes`);
  cy.get(`[data-filter-value*="${filterValue}"]`)
    .find('a:contains("Run")')
    .first()
    .click();
  cy.get("#modalOkButton").click();
});

Cypress.Commands.add("enableFeature", ({ prefix }, filterValue) => {
  cy.visit(`${prefix}/Admin/Features`);
  cy.get(`[data-filter-value*="${filterValue}"]`)
    .find('a:contains("Enable")')
    .first()
    .click();
});
