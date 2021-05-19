/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("Scheduling app tests", function() {    
  
  it("Scheduling app setup is successfull", function() {
    const tenant = generateTenantInfo("scheduling-app", "Scheduling app recipe");
    cy.newTenant(tenant);
    cy.login(tenant);
  })
});
