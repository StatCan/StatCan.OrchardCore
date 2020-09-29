/// <reference types="Cypress" />

import { creds } from "../support/objects";

const sassCreds = {
  name: "Testing SaaS",
  setupRecipe: "SaaS",
  ...creds
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
 