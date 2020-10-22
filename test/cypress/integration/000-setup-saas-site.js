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
});
 