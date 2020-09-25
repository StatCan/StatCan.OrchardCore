/// <reference types="Cypress" />

import { creds } from "../../support/objects";

export const formsSite = {
  name: "a1",
  prefix: "a1",
  setupRecipe: "bootstrap-setup-recipe",
  ...creds
};

describe("VueForm setup", function() {
  it("Setup VueForm test tenant", function() {
    cy.login();
    cy.createTenant(formsSite);
    cy.gotoTenantSetup(formsSite);
    cy.setupSite(formsSite);
    cy.login(formsSite);
    cy.enableFeature(formsSite, "VueForm");
    cy.enableFeature(formsSite, "VueForms Localized");
  });
});
