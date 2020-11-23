/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("MultivalueTextField Test", function() {    
  it("Create MultivalueTextField tenant ", function() {
    let tenant = generateTenantInfo("bootstrap-theme-setup", "MultivalueTextField Feature enabled")
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.enableFeature(tenant, "StatCan_OrchardCore_ContentFields_MultiValueTextField");
    // todo add some widget that uses the feature, and do some type of assertions.
  })
});
