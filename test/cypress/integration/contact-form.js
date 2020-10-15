/// <reference types="Cypress" />
import { generateTenantInfo } from '../support/objects';

describe("ContactForm Test", function() {    
  it("Create ContactForm tenant ", function() {
    let tenant = generateTenantInfo("bootstrap-theme-setup")
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.runRecipe(tenant, 'VueForms Contact form example');
  })

});
