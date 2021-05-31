// ***********************************************
// https://on.cypress.io/custom-commands
// ***********************************************
Cypress.Commands.add("loginAs", function(tenantPrefix, userName, pasword) {
  const config = Cypress.config('orchard');
  cy.visit(`${tenantPrefix}/login`);
  cy.get("#UserName").type(userName);
  cy.get("#Password").type(pasword);
  cy.get("#UserName").closest('form').submit();
});
