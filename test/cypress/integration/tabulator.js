/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("Table Creator Test", function() {   
  let tenant;
  it("Create Tabulator tenant ", function() {
    tenant = generateTenantInfo("bootstrap-theme-setup", "Tabulatormodule tests")
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.enableFeature(tenant, "StatCan_OrchardCore_Tabulator");
  })

});