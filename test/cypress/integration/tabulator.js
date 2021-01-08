/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

describe("Tabulator Test", function() {   
  let tenant;
  it("Create Tabulator tenant ", function() {
    tenant = generateTenantInfo("bootstrap-theme-setup", "Tabulatormodule tests")
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.enableFeature(tenant, "StatCan_OrchardCore_Tabulator");
  })

   //Run Tabulator Recipe
   it("Can run Tabulator Recipe", function() {
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.runRecipe(tenant, 'Tabulator');
})

  //Add Tabulator widget
  it("Can create a table", function() {
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.visit(`${tenant.prefix}/Admin/Contents/ContentTypes/Tabulator/create`);
    cy.get("input[name='Tabulator.TableData.Text']").type('[ {{}id:1, name:"Oli Bob", progress:12, gender:"male", rating:1, col:"red", dob:"19/02/1984", car:1}, {{}id:2, name:"Mary May", progress:1, gender:"female", rating:2, col:"blue", dob:"14/05/1982", car:true}, {{}id:3, name:"Christine Lobowski", progress:42, gender:"female", rating:0, col:"green", dob:"22/05/1982", car:"true"}, {{}id:4, name:"Brendon Philips", progress:100, gender:"male", rating:1, col:"orange", dob:"01/08/1980"}, {{}id:5, name:"Margret Marmajuke", progress:16, gender:"female", rating:5, col:"yellow", dob:"31/01/1999"}, {{}id:6, name:"Frank Harbours", progress:38, gender:"male", rating:4, col:"red", dob:"12/05/1966", car:1}]', {force:true});
    cy.get("input[name='Tabulator.ColumnsData.Text']").type('[ {{}title:"Name", field:"name", editor:"input"},{{}title:"Task Progress", field:"progress", hozAlign:"left", formatter:"progress", editor:true},{{}title:"Gender", field:"gender", width:95, editor:"select", editorParams:{{}values:["male", "female"]}}, {{}title:"Rating", field:"rating", formatter:"star", hozAlign:"center", width:100, editor:true},         {{}title:"Color", field:"col", width:130, editor:"input"}, {{}title:"Date Of Birth", field:"dob", width:130, sorter:"date", hozAlign:"center"}, {{}title:"Driver", field:"car", width:90,  hozAlign:"center", formatter:"tickCross", sorter:"boolean", editor:true}]', {force:true});
    cy.get("input[name='Tabulator.AutoColumnizer.Text']").type('false', {force:true});
    cy.get("input[name='Tabulator.PaginationSize.Text']").type('5', {force:true});
    cy.get('.btn-success').click();
  })

  //View Tabulator
  it("Can view the Table", function() {
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.visit(`${tenant.prefix}/Admin/Contents/ContentItems`);
    cy.get('.float-right').click();
  })


});