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

Cypress.Commands.overwrite("uploadRecipeJson", (originalFn, {prefix}, fixturePath) => {
  cy.fixture(fixturePath).then((data) => {
    cy.visit(`${prefix}/Admin/DeploymentPlan/Import/Json`);
    cy.get('.CodeMirror').should('be.visible');
    cy.get("body").then($body => {
      const elem = $body.find(".CodeMirror")[0].CodeMirror.setValue(JSON.stringify(data));
    });
    cy.get('.ta-content > form').submit();
    // make sure the message-success alert is displayed
    cy.get('.message-success').should('contain', "Recipe imported");
  });
});
