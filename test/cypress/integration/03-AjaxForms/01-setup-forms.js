/// <reference types="Cypress" />

import { creds } from "../../support/objects";

export const blank = {
  name: "af5",
  prefix: "af5",
  setupRecipe: "Blank",
  ...creds
};

describe("Blank setup", function() {
  it.only("Setup AjaxForms test tenant", function() {
    cy.login();
    cy.createTenant(blank);
    cy.gotoTenantSetup(blank);
    cy.setupSite(blank);
    cy.login(blank);
    cy.enableFeature(blank, "AjaxForms");
  });
  it("Create a simple form"), function() {
    cy.login(blank);

  }
});
