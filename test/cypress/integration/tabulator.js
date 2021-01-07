/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("Tabulator Test", function() {    
  it("Create Tabulator tenant ", function() {
    let tenant = generateTenantInfo("digital-theme-setup", "Tabulator module tests")
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.enableFeature(tenant, "StatCan_OrchardCore_Tabulator");
  })
});