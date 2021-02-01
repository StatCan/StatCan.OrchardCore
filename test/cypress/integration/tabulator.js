/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("Table Creator Test", function() {   
  let tenant;
  it("Create Tabulator tenant ", function() {
    tenant = generateTenantInfo("bootstrap-theme-setup", "Tabulatormodule tests")
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.enableFeature(tenant, "StatCan_OrchardCore_Tabulator");
  })

   //Run Tabulator Recipe
   it("Can run TableCreator Recipe", function() {
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.runRecipe(tenant, 'Table_Creator');
})

  //Add Tabulator widget
  it("Can create a table", function() {
    cy.fixture('tableCreatorData.json').as('dataJSON')
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.visit(`${tenant.prefix}/Admin/Contents/ContentTypes/TableCreator/create`);
    cy.fixture('tableCreatorData.json').then((dataJSON) => {
      cy.get("input[name='TableCreator.TableData.Text']")
        .type(JSON.stringify(dataJSON),{ force: true, parseSpecialCharSequences: false })
    })
    cy.fixture('tableCreatorColData.json').then((dataJSON) => {
      cy.get("input[name='TableCreator.ColumnsData.Text']")
      .type(JSON.stringify(dataJSON), { force: true, parseSpecialCharSequences: false })
    })
    cy.get("input[name='TableCreator.AutoColumnizer.Text']").type('false', {force:true});
    cy.get("input[name='TableCreator.PaginationSize.Value']").type('5', {force:true});
    cy.get('.btn-success').click();
  })

  //View Tabulator
  it("Can view the Table", function() {
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.visit(`${tenant.prefix}/Admin/Contents/ContentItems`);
    cy.get('.float-right > .btn-success').click();
  })


});