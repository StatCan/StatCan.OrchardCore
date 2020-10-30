/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/utils';

describe("Common types Test", function() {    
  it("Create CommonTypes tenant ", function() {
    let tenant = generateTenantInfo("bootstrap-theme-setup", "Common Types module setup")
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.enableFeature(tenant, "StatCan_OrchardCore_CommonTypes_Pages");
    cy.enableFeature(tenant, "StatCan_OrchardCore_CommonTypes_Widgets");
  })
});
