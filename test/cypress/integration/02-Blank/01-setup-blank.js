/// <reference types="Cypress" />

import { creds } from "../../support/objects";

export const blank = {
  name: "Blank",
  prefix: "blank",
  setupRecipe: "Blank",
  ...creds
};

describe("Blank setup", function() {
  it("Setup blank tenant", function() {
    cy.login();
    cy.createTenant(blank);
    cy.gotoTenantSetup(blank);
    cy.setupSite(blank);
  });
});
