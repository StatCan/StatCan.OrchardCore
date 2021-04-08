/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("Vuetify theme SaaS tests", function() {    
  let tenant;

  before(() => {
    tenant = generateTenantInfo("vuetify-saas-setup")
    cy.newTenant(tenant);
  })

  it("Vuetify SaaS recipe is successfull", function() {
    cy.login(tenant);
  })
});
