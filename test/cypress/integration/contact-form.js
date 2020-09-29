/// <reference types="Cypress" />
import { generateTenantInfo } from '../support/objects';

const contentId = "45kdfafn7sv6racbrhaarghjma"

describe("ContactForm Test", function() {    
  it("Create ContactForm tenant ", function() {
    let tenant = generateTenantInfo("bootstrap-setup-recipe")
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.runRecipe(tenant, 'VueForms Contact form example');
  })

});
