/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/utils';

describe("Portal theme tests", function() {    
  let tenant;

  before(() => {
    // generate a tenant for all tests below
    tenant = generateTenantInfo("portal-theme-setup")
    cy.newTenant(tenant);
    cy.login(tenant);
  })
  it("Recipe is successfull", function() {
    cy.login(tenant);
  })
});
