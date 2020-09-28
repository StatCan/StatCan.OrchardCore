/// <reference types="Cypress" />
import { generateTenantInfo } from '../support/objects';

const contentId = "45kdfafn7sv6racbrhaarghjma"

describe("VueForm Tests", function() {    
  let tenant;

  before(() => {
      // generate a tenant for all tests below
      tenant = generateTenantInfo("bootstrap-setup-recipe")
      cy.newTenant(tenant);
      cy.login(tenant);
      cy.uploadRecipeJson(tenant, "recipes/vue-form.json");
  })

  it("Client side validation prevents submit", function() {
    cy.visitContent(tenant, contentId);

    // submit the form
    cy.getByCy('form-button-submit').click();

    cy.getByCy('form-field-email').closest('.v-input').find('.v-messages__message').should('contain', 'The email field is required');
    cy.getByCy('form-field-gender').closest('.v-input').find('.v-messages__message').should('contain', 'The gender field is required');
  })

  it("Server side validation highlights the error", function() {
    
    cy.visitContent(tenant, contentId);

    cy.getByCy('form-field-email').type('email@email.com', {force:true});
    cy.getByCy('form-field-gender').click({force:true}).type('{downarrow}{enter}', {force:true});

    cy.getByCy('form-button-submit').click();
    cy.getByCy('form-field-name').closest('.v-input').find('.v-messages__message').should('contain', 'The name is required');
    cy.getByCy('form-errormessage').contains('An error occurred while submitting your form.');
  })

  it("Clear buttons clears the validation errors", function() {
    cy.visitContent(tenant, contentId);

    cy.getByCy('form-button-submit').click();
    cy.getByCy('form-field-email').closest('.v-input').find('.v-messages__message').should('contain', 'The email field is required');
    cy.getByCy('form-field-gender').closest('.v-input').find('.v-messages__message').should('contain', 'The gender field is required');

    cy.getByCy('form-button-clear').click();

    cy.getByCy('form-field-email').closest('.v-input').find('.v-messages__message').should('not.exist'); 
    cy.getByCy('form-field-gender').closest('.v-input').find('.v-messages__message').should('not.exist'); 
  })

  it("Submit works and displays success message", function() {
    cy.visitContent(tenant, contentId);

    cy.getByCy('form-field-name').type('Jean-Philippe Tissot', {force:true});
    cy.getByCy('form-field-email').type('jp@email.com', {force:true});
    cy.getByCy('form-field-gender').click({force:true}).type('{downarrow}{downarrow}{enter}', {force:true});

    cy.getByCy('form-button-submit').click();
    cy.getByCy('form-successmessage').contains('Your form was submitted successfully.');
  })

  it("Submit works, creates a ContenItem and redirects", function() {
    cy.visitContent(tenant, "44w9hn15s953d23n4pak4fm3n4");

    cy.getByCy('form-field-name').type('Jean-Philippe Tissot', {force:true});
    cy.getByCy('form-field-email').type('jp@email.com', {force:true});
    cy.getByCy('form-field-gender').click({force:true}).type('{downarrow}{downarrow}{enter}', {force:true});
    cy.getByCy('form-button-submit').click();

    cy.get('.field-name-person-info-name').should('contain', 'Jean-Philippe Tissot');
    cy.get('.field-name-person-info-email').should('contain', 'jp@email.com');
    cy.get('.field-name-person-info-gender').should('contain', 'Male');
  })

  it("Localization works", function() {
    cy.visitContent(tenant, contentId);

    cy.getByCy('form-field-name').closest('.v-input').find('.v-label').should('contain', "Name");
    cy.get('#navbar-toggle-button').click();
    cy.getByCy('form-field-name').closest('.v-input').find('.v-label').should('contain', "Nom");
  })
});
