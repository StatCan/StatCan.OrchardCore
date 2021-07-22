/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("UserProfile Test", function() {    
  it("Create Create UserProfile form tenant ", function() {
    let tenant = generateTenantInfo("vuetify-theme-setup", "VueForm UserProfile form example")
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.runRecipe(tenant, 'VueForm_UserProfile');
  })
});
