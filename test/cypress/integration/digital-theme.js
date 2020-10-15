/// <reference types="Cypress" />
import { generateTenantInfo } from '../support/objects';

describe("Digital theme tests", function() {    
  let tenant;

  before(() => {
    // generate a tenant for all tests below
    tenant = generateTenantInfo("digital-theme-setup")
    cy.newTenant(tenant);
    cy.login(tenant);
  })
  it("Recipe is successfull", function() {
    cy.login(tenant);
  })
});
