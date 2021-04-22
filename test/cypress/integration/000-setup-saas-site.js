/// <reference types="Cypress" />

const sassCreds = {
  name: "Testing SaaS",
  setupRecipe: "SaaS",
  prefix: ""
}

describe("SaaS site setup", function() {
  it("SaaS tenant setup", function() {
    cy.visit("/");
    cy.siteSetup(sassCreds);
    cy.login(sassCreds);
    cy.setPageSize(sassCreds,"100");
    cy.enableFeature(sassCreds, "StatCan_OrchardCore_SaaSConfiguration");
  });
});
 