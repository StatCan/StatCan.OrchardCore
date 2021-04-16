/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("SaaSConfiguration module tests", function() {    
  let tenant;

  before(() => {
    tenant = generateTenantInfo("vuetify-theme-setup");
    cy.newTenant(tenant);
  })

  it("SaaSConfiguration module sets up the openid server properly", function() {
    cy.login(tenant);
    cy.enableFeature(tenant, "StatCan_OrchardCore_SaaSConfiguration_Client");
  })
  
});
