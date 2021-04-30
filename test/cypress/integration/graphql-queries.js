/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("GraphQL Query test", function() {    
  
  it("Test that GraphQL Query executes successfully", function() {
    const tenant = generateTenantInfo("vuetify-theme-setup", "Simple vuetify theme");
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.uploadRecipeJson(tenant, "recipes/gql-query.json");
    cy.visit(`${tenant.prefix}/showquery`);
    cy.get('pre').should('contain', `"contentItemId": "4twvqztdaw0757733s6zmhsddn"`)
    cy.get('pre').should('contain', `"contentItemId": "4e4ce8jg09dgg42jw25ptphgwj"`)
  })
});
