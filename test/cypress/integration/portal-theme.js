/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("Portal theme tests", function() {    
  let tenant;

  before(() => {
    tenant = generateTenantInfo("portal-theme-setup")
    cy.newTenant(tenant);
  })

  it("Portal theme recipe is successfull", function() {
    cy.login(tenant);
  })
});
