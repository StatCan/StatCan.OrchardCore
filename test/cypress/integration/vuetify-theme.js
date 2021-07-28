/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("Vuetify theme tests", function() {    
  
  it("Vuetify theme recipe is successfull", function() {
    const tenant = generateTenantInfo("vuetify-theme-setup", "Simple vuetify theme");
    cy.newTenant(tenant);
    cy.login(tenant);
  })
});
