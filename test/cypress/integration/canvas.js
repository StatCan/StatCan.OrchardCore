import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

before(function () {
    cy.fixture('businessCanvas.json').then(function (dataJSON) {
      this.data = dataJSON;
    })
  })

  describe("Canvas Test", function() {    
    let tenant;
    it("Create Canvas tenant ", function() {
      tenant = generateTenantInfo("bootstrap-theme-setup", "SurveyJS module tests")
      cy.newTenant(tenant);
      cy.login(tenant);
      cy.enableFeature(tenant, "StatCan_OrchardCore_Canvas");
    })
  
    //Run Canvas Recipe
    it("Can run Canvas Recipe", function() {
      cy.visit(`${tenant.prefix}/login`)
      cy.login(tenant);
      cy.runRecipe(tenant, 'Canvas');
  })

    //Create a Business Canvas
    it("Can create a Business type Canvas", function() {
        cy.fixture('businessCanvas.json').as('dataJSON')
        cy.visit(`${tenant.prefix}/login`)
        cy.login(tenant);
        cy.visit(`${tenant.prefix}/Admin/Contents/ContentTypes/Canvas/create`);
        cy.get('[type="radio"]').check("business", {force: true})
        cy.fixture('businessCanvas.json').then((dataJSON) => {
          cy.get('.CodeMirror textarea')
            .type(JSON.stringify(dataJSON),{ force: true, parseSpecialCharSequences: false })
        })
        cy.get('.btn-success').click();
      })

    //Create a Client Canvas
    it("Can create a Client type Canvas", function() {
        cy.fixture('clientCanvas.json').as('dataJSON')
        cy.visit(`${tenant.prefix}/login`)
        cy.login(tenant);
        cy.visit(`${tenant.prefix}/Admin/Contents/ContentTypes/Canvas/create`);
        cy.get('[type="radio"]').check("client", {force: true})
        cy.fixture('clientCanvas.json').then((dataJSON) => {
        cy.get('.CodeMirror textarea')
            .type(JSON.stringify(dataJSON),{ force: true, parseSpecialCharSequences: false })
        })
        cy.get('.btn-success').click();
  })

    //View Business and Client Canvas
    it("Can view Business and Client Canvas", function() {
        cy.visit(`${tenant.prefix}/login`)
        cy.login(tenant);
        cy.visit(`${tenant.prefix}/Admin/Contents/ContentItems`);
        cy.get('.btn-success').click({multiple: true});
    })
      
});