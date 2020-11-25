/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("Hackathon SaaS tests", function() {    
  let tenant;

  before(() => {
    tenant = generateTenantInfo("hackathon-saas-setup")
    cy.newTenant(tenant);
  })

  it("Hackathon SaaS recipe is successfull", function() {
    cy.login(tenant);
  })
});
