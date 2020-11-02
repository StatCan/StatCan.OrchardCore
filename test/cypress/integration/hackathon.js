/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/utils';

describe("Hackathon Tests", function() {    
  let tenant;

  before(() => {
    tenant = generateTenantInfo("hackathon-theme-setup")
    cy.newTenant(tenant);
  })
  
  it("Can login to Hackathon site", function() {
    cy.login(tenant);
  })
});
