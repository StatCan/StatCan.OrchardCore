/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("SurveyJS Test", function() {    
  let tenant;
  it("Create SurveyJS tenant ", function() {
    tenant = generateTenantInfo("bootstrap-theme-setup", "SurveyJS module tests")
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.enableFeature(tenant, "StatCan_OrchardCore_SurveyJS");
  })

})
