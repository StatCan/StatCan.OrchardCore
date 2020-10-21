/// <reference types="Cypress" />

const sassCreds = {
  name: "Testing SaaS",
  setupRecipe: "SaaS",
}

describe("SaaS site setup", function() {
  it("SaaS tenant setup", function() {
    cy.visit("/");
    cy.setupSite(sassCreds);
    cy.login(sassCreds)
    cy.setPageSize(sassCreds,"100");
  });
  // for the tenants page. To easily be able to see all testing tenants
});
 