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
Cypress.Commands.add('newTenant', function(tenantInfo) {
  cy.login();
  cy.createTenant(tenantInfo);
  cy.gotoTenantSetup(tenantInfo);
  cy.setupSite(tenantInfo);
});

Cypress.Commands.add("createTenant", ({ name, prefix }) => {
  // We create tenants on the SaaS tenant
  cy.visit("/Admin/Tenants");
  // weak selector. CreateTenant
  cy.get('form > .row > .form-group > .btn-group > .btn').click()
  cy.get("#Name").type(name, {force:true});
  cy.get("#RequestUrlPrefix").type(prefix, {force:true});

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
Cypress.Commands.add("uploadRecipeJson", ({ prefix }, fixturePath) => {
  cy.fixture(fixturePath).then((data) => {
    cy.visit(`${prefix}/Admin/DeploymentPlan/Import/Json`);
    cy.get('#Json').invoke('val', JSON.stringify(data)).trigger('input',{force: true});
    cy.get('.ta-content > form').submit();
    // make sure the message-success alert is displayed
    cy.get('.message-success').should('contain', "Recipe imported");
  }); 
});

Cypress.Commands.add("visitContent", ({ prefix }, contentItemId) => {
  cy.visit(`${prefix}/Contents/ContentItems/${contentItemId}`);
});

export function byCy(id, exact) {
  if (exact) {
    return `[data-cy="${id}"]`;
  }
  return `[data-cy^="${id}"]`;
}


Cypress.Commands.add('getByCy', (selector, exact = false) => {
  return cy.get(byCy(selector, exact));
});

Cypress.Commands.add(
  'findByCy',
  {prevSubject: 'optional'},
  (subject, selector, exact = false) => {
    return subject
      ? cy.wrap(subject).find(byCy(selector, exact))
      : cy.find(byCy(selector, exact));
  },
);

Cypress.Commands.add("setPageSize", ({ prefix = '' }, size) => {
  cy.visit(`${prefix}/Admin/Settings/general`);
  cy.get('#ISite_PageSize')
    .clear()
    .type(size);
  cy.get('#ISite_PageSize').parents('form').submit();
  // wait until the success message is displayed
  cy.get('.message-success');
});


Cypress.Commands.add('visitAdmin', ({ prefix }) => {
  cy.visit(`${prefix}/Admin`);
});
Cypress.Commands.add('visitTenantPage', ({ prefix }, url) => {
  cy.visit(`${prefix}/${url}`);
});
