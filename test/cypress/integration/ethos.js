/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("Assessment Test", function() {    
  it("Create Assessment tenant ", function() {
    let tenant = generateTenantInfo("digital-theme-setup", "Ethos module tests")
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.enableFeature(tenant, "StatCan_OrchardCore_Ethos");
  })
});