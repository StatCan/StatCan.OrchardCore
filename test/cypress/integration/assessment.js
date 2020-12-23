/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

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
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.visit(`${tenant.prefix}/Admin/Contents/ContentTypes/Assessment/create`);
    cy.get("input[name='Assessment.Data.Text']").type('{{}"completedHtml": "Thank you for completing the assessment","title": "Confidentiality Classification Tool","description": "Methodology: The risk assessment of a selected product is performed by using a checklist to associate one of three ratings, LOW, MEDIUM or HIGH, with how much effort (SIGNIFICANT, SOME or LITTLE) that a third party must expend in order to disclose confidential information once the product has become available.","pages": [{{}"name": "page1","elements": [{{}"type": "radiogroup","name": "question1","title": "Can confidential information be revealed directly through the information displayed by the product? ","isRequired": true,"choices": [{{}"value": "item1-1","text": "LOW risk as SIGNIFICANT effort is required","score": 1},{{}"value": "item2-2","text": "MEDIUM risk as SOME effort is required","score": 2},{{}"value": "item3-3","text": "HIGH risk as LITTLE effort is required","score": 3}]},{{}"type": "radiogroup","name": "question4","title": "Can confidential information be revealed by grouping attributes available in the product?","isRequired": true,"choices": [{{}"value": "item1","text": "LOW risk as SIGNIFICANT effort is required","score": 1},{{}"value": "item2","text": "MEDIUM risk as SOME effort is required","score": 2},{{}"value": "item3","text": "HIGH risk as LITTLE effort is required","score": 3}]},{{}"type": "radiogroup","name": "question3","title": "Can confidential information be revealed through a probabilistic statement derived from the information contained in the product?","isRequired": true,"choices": [{{}"value": "item1","text": "LOW risk as SIGNIFICANT effort is required","score": 1},{{}"value": "item2","text": "MEDIUM risk as SOME effort is required","score": 2},{{}"value": "item3","text": "HIGH risk as LITTLE effort is required","score": 3}]},{{}"type": "radiogroup","name": "question2","title": "Can confidential information be revealed by combining the information contained in this product with that from other products?","isRequired": true,"choices": [{{}"value": "item1","text": "LOW risk as SIGNIFICANT effort is required","score": 1},{{}"value": "item2","text": "MEDIUM risk as SOME effort is required","score": 2},{{}"value": "item3","text": "HIGH risk as LITTLE effort is required","score": 3}]}],"title": "Disclosure Risk Self-Assessment Checklist","description": "PURPOSE: Provide a self-assessment of the disclosure risk associated with the product under examination."},{{}"name": "page2","elements": [{{}"type": "radiogroup","name": "question5","title": "Select the highest level of impact a breach would have on an individual, business, or institution from the Sensitivity Rating Legend below, and provide a justification for why you believe this is the case.  The impact must reflect an outcome that can reasonably be expected to occur, rather than one based on an extreme scenario.","isRequired": true,"choices": [{{}"value": "item1","text": "Severe (5)","score": 5},{{}"value": "item2","text": "High (4)","score": 4},{{}"value": "item3","text": "Medium (3)","score": 3},{{}"value": "item4","text": "Low (2)","score": 2}],"hasOther": true,"otherText": "Negligible (1)","score": 1}],"title": "Sensitivity Self-Assessment Checklist","description": "To assess the level of sensitivity of the specified product based on the impact a breach would have on an individual, business or institution."}]}', {force:true});
    cy.get('.btn-success').click();
  })

  //View Assessment
  it("Can view the Assessment form", function() {
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.visit(`${tenant.prefix}/Admin/Contents/ContentItems`);
    cy.get('.float-right').click();
  })
});
