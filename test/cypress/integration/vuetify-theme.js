/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("Vuetify theme tests", function() {    
  
  it.only("Vuetify theme recipe is successfull", function() {
    const tenant = generateTenantInfo("vuetify-theme-setup", "Simple vuetify theme");
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.enableFeature(tenant, "StatCan_OrchardCore_Scheduling");

  })

  it("Vuetify SaaS recipe is successfull", function() {
    const tenant = generateTenantInfo("vuetify-saas-setup");
    cy.newTenant(tenant);
    cy.login(tenant);
  })


});
