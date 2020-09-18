/// <reference types="Cypress" />

import { bootstrap } from './01-setup-forms';

describe("VueForm Tests", function() {
  it("Creates form"), function() {
    cy.login(bootstrap);
    
  }
});
