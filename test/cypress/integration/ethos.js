/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("Persona Test", function() {    
  let tenant;
  it("Create Persona tenant ", function() {
    tenant = generateTenantInfo("bootstrap-theme-setup", "Persona module tests")
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.enableFeature(tenant, "StatCan_OrchardCore_Persona");
  })

  //Run Persona Recipe
  it("Can run Persona Recipe", function() {
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.runRecipe(tenant, 'Persona');
  })

  //Add Persona widget
  it("Can create a Persona", function() {
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.visit(`${tenant.prefix}/Admin/Contents/ContentTypes/Persona/create`);
    cy.fixture('softDev.json').then((dataJSON) => {
      cy.get('.CodeMirror textarea')
        .type(JSON.stringify(dataJSON),{ force: true, parseSpecialCharSequences: false })
    })    
    cy.get('.btn-success').click();
  })

  //View Assessment
  it("Can view the Persona form", function() {
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.visit(`${tenant.prefix}/Admin/Contents/ContentItems`);
    cy.get('.float-right > .btn-success').click();
  })


});