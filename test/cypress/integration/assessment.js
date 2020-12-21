/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("Assessment Test", function() {    
  it("Create Assessment tenant ", function() {
    let tenant = generateTenantInfo("bootstrap-theme-setup", "Assessment module tests")
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.enableFeature(tenant, "StatCan_OrchardCore_Assessment");
  })

  //Create Assessment Content Type



});
