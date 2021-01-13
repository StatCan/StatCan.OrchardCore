/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';


before(function () {
  cy.fixture('assessment.json').then(function (dataJSON) {
    this.data = dataJSON;
  })
})

describe("Assessment Test", function() {    
  let tenant;
  it("Create Assessment tenant ", function() {
    tenant = generateTenantInfo("bootstrap-theme-setup", "Assessment module tests")
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.enableFeature(tenant, "StatCan_OrchardCore_Assessment");
  })

  //Run Assessment Recipe
  it("Can run Assessment Recipe", function() {
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.runRecipe(tenant, 'Assessment');
})

  //Add Assessment widget
  it("Can create an assessment form", function() {
    cy.fixture('assessment.json').as('dataJSON')
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.visit(`${tenant.prefix}/Admin/Contents/ContentTypes/Assessment/create`);
    cy.fixture('assessment.json').then((dataJSON) => {
      cy.get("input[name='Assessment.Data.Text']")
        .type(JSON.stringify(dataJSON),{ force: true, parseSpecialCharSequences: false })
    })
    cy.get('.btn-success').click();
  })

  //View Assessment
  it("Can view the Assessment form", function() {
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.visit(`${tenant.prefix}/Admin/Contents/ContentItems`);
    cy.get('.float-right > .btn-success').click();
  })
})
