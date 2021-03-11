import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

before(function () {
  cy.fixture('surveyjs.json').then(function (dataJSON) {
    this.data = dataJSON;
  })
})

describe("SurveyJS Test", function() {    
  let tenant;
  it("Create SurveyJS tenant ", function() {
    tenant = generateTenantInfo("bootstrap-theme-setup", "SurveyJS module tests")
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.enableFeature(tenant, "StatCan_OrchardCore_SurveyJS");
  })

  //Run SurveyJS Recipe
  it("Can run SurveyJS Recipe", function() {
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.runRecipe(tenant, 'SurveyJS');
})

  //Add SurveyJS widget
  it("Can create an SurveyJS form", function() {
    cy.fixture('surveyjs.json').as('dataJSON')
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.visit(`${tenant.prefix}/Admin/Contents/ContentTypes/SurveyJS/create`);
    cy.fixture('surveyjs.json').then((dataJSON) => {
      cy.get('.CodeMirror textarea')
        .type(JSON.stringify(dataJSON),{ force: true, parseSpecialCharSequences: false })
    })
    cy.get('.btn-success').click();
  })

  //View Assessment
  it("Can view the Assessment form", function() {
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.visit(`${tenant.prefix}/Admin/Contents/ContentItems`);
    cy.get('.btn-success').click();
  })
});