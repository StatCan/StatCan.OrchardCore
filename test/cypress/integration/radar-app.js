/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("Radar app tests", function() {    
  
  it("Radar app setup is successfull", function() {
    const tenant = generateTenantInfo("radar-setup", "Radar app recipe");
    cy.newTenant(tenant);
    cy.login(tenant);
  })
});
