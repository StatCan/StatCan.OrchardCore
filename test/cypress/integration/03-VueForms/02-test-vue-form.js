/// <reference types="Cypress" />

import { formsSite } from './01-setup-forms';

const contentId = "45kdfafn7sv6rrcbrhtarghjmj"

describe.only("VueForm Tests", function() {
  it("Upload form", function() {
    cy.login(formsSite);
    cy.uploadRecipeJson(formsSite, "recipes/vue-form.json");
    cy.visitContent(formsSite, contentId);
    // form exists
    cy.getByCy('form-container');

  })

  it("Client side validation prevents submit", function() {
    cy.visitContent(formsSite, contentId);

    // submit the form
    cy.getByCy('form-button-submit').click();

    cy.getByCy('form-field-email').closest('.v-input').find('.v-messages__message').should('contain', 'The email field is required');
    cy.getByCy('form-field-gender').closest('.v-input').find('.v-messages__message').should('contain', 'The gender field is required');
  })
  it.only("Server side validation highlights the error", function() {
    
    cy.visitContent(formsSite, contentId);

    cy.getByCy('form-field-email').type('email@email.com', {force:true});
    cy.getByCy('form-field-gender').click({force:true}).type('{downarrow}{enter}', {force:true});

    cy.getByCy('form-button-submit').click();
  })
  it("Clear buttons works", function() {
 
  })
  it("Localization works", function() {
 
  })
});
