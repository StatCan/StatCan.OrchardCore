/// <reference types="Cypress" />

import { creds } from "../../support/objects";

export const bootstrap = {
  name: "vueform7",
  prefix: "vueform7",
  setupRecipe: "bootstrap-setup-recipe",
  ...creds
};

describe("VueForm setup", function() {
  it.only("Setup VueForm test tenant", function() {
    cy.login();
    cy.createTenant(bootstrap);
    cy.gotoTenantSetup(bootstrap);
    cy.setupSite(bootstrap);
    cy.login(bootstrap);
    cy.enableFeature(bootstrap, "VueForm");
    cy.enableFeature(bootstrap, "VueForms Localized");
  });
});
