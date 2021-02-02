/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("Assessment Test", function() {    
  let tenant;
  it("Create Assessment tenant ", function() {
    tenant = generateTenantInfo("bootstrap-theme-setup", "Assessment module tests")
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.enableFeature(tenant, "StatCan_OrchardCore_Assessment");
  })

  //Run Assessment Recipe
  it("Can run Assessment Recipe", function() {
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.runRecipe(tenant, 'Assessment');
})

})
