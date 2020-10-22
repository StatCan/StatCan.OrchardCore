/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/utils';

describe("Digital theme tests", function() {    
  let tenant;

  before(() => {
    tenant = generateTenantInfo("digital-theme-setup")
    cy.newTenant(tenant);
  })

  it("Digital setup recipe is successfull", function() {
    cy.login(tenant);
  })
});
