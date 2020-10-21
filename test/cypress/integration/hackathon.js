/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/utils';

describe("Hackathon Tests", function() {    
  let tenant;

  before(() => {
    // generate a tenant for all tests below
    tenant = generateTenantInfo("hackathon-setup-recipe")
    cy.newTenant(tenant);
    cy.login(tenant);
  })
  it("Can login to Hackathon site", function() {
    cy.login(tenant);
  })
});
