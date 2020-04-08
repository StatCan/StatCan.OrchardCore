/// <reference types="Cypress" />

import { creds } from "../../support/objects";

describe("SaaS site setup", function() {
  it("SaaS tenant setup", function() {
    cy.visit("/");
    cy.setupSite({
      name: "Inno SaaS",
      setupRecipe: "SaaS",
      ...creds
    });
  });

  //todo check title and menu maybe ?
});
