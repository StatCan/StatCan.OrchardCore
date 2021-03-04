/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("Persona Test", function() {    
  it("Create Persona tenant ", function() {
    let tenant = generateTenantInfo("bootstrap-theme-setup", "Persona module tests")
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.enableFeature(tenant, "StatCan_OrchardCore_Persona");
  })
});