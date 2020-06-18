/// <reference types="Cypress" />

import { creds } from "../../support/objects";

export const blank = {
  name: "AjaxForm3",
  prefix: "ajaxform3",
  setupRecipe: "Blank",
  ...creds
};

describe("Blank setup", function() {
  it("Setup AjaxForms test tenant", function() {
    cy.login();
    cy.createTenant(blank);
    cy.gotoTenantSetup(blank);
    cy.setupSite(blank);
    cy.login(blank);
    cy.enableFeature(blank, "AjaxForms");
  });
});
