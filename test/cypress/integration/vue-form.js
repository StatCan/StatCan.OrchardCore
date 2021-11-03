/// <reference types="Cypress" />
import { generateTenantInfo } from "cypress-orchardcore/dist/utils";

const contentId = "45kdfafn7sv6racbrhaarghjma";
const bagFormId = "42pzsb5rs6vd3zj7n5z88se7dd";

describe("VueForm Tests", function() {
  let tenant;

  before(() => {
    tenant = generateTenantInfo("bootstrap-theme-setup", "VueForm tests");
    cy.newTenant(tenant);
    cy.login(tenant);
    //cy.enableFeature(tenant, "OrchardCore_Workflows_Http");
    cy.uploadRecipeJson(tenant, "recipes/vue-form.json");
  });

  it("Client side validation prevents submit", function() {
    cy.visitContentPage(tenant, contentId);

    // submit the form
    cy.getByCy("form-button-submit").click();

    cy.getByCy("form-field-email")
      .closest(".v-input")
      .find(".v-messages__message")
      .should("contain", "The email field is required");
    cy.getByCy("form-field-gender")
      .closest(".v-input")
      .find(".v-messages__message")
      .should("contain", "The gender field is required");
  });

  it("Server side validation highlights the error", function() {
    cy.visitContentPage(tenant, contentId);

    cy.getByCy("form-field-email").type("email@email.com", { force: true });
    cy.getByCy("form-field-gender")
      .click({ force: true })
      .type("{downarrow}{enter}", { force: true });

    cy.getByCy("form-button-submit").click();
    cy.getByCy("form-field-name")
      .closest(".v-input")
      .find(".v-messages__message")
      .should("contain", "The name is required");
    cy.getByCy("form-errormessage").contains(
      "An error occurred while submitting your form."
    );
  });

  it("Clear buttons clears the validation errors", function() {
    cy.visitContentPage(tenant, contentId);

    cy.getByCy("form-button-submit").click();
    cy.getByCy("form-field-email")
      .closest(".v-input")
      .find(".v-messages__message")
      .should("contain", "The email field is required");
    cy.getByCy("form-field-gender")
      .closest(".v-input")
      .find(".v-messages__message")
      .should("contain", "The gender field is required");

    cy.getByCy("form-button-clear").click();

    cy.getByCy("form-field-email")
      .closest(".v-input")
      .find(".v-messages__message")
      .should("not.exist");
    cy.getByCy("form-field-gender")
      .closest(".v-input")
      .find(".v-messages__message")
      .should("not.exist");
  });

  it("Submit works and displays success message", function() {
    cy.visitContentPage(tenant, contentId);

    cy.getByCy("form-field-name").type("Jean-Philippe Tissot", { force: true });
    cy.getByCy("form-field-email").type("jp@email.com", { force: true });
    cy.getByCy("form-field-gender")
      .click({ force: true })
      .type("{downarrow}{downarrow}{enter}", { force: true });

    cy.getByCy("form-button-submit").click();
    cy.getByCy("form-successmessage").contains(
      "Your form was submitted successfully."
    );
  });

  it("Submit works, creates a ContenItem and redirects", function() {
    cy.visitContentPage(tenant, "44w9hn15s953d23n4pak4fm3n4");

    cy.getByCy("form-field-name").type("Jean-Philippe Tissot", { force: true });
    cy.getByCy("form-field-email").type("jp@email.com", { force: true });
    cy.getByCy("form-field-gender")
      .click({ force: true })
      .type("{downarrow}{downarrow}{enter}", { force: true });
    cy.getByCy("form-button-submit").click();

    cy.get(".field-name-person-info-name").should(
      "contain",
      "Jean-Philippe Tissot"
    );
    cy.get(".field-name-person-info-email").should("contain", "jp@email.com");
    cy.get(".field-name-person-info-gender").should("contain", "Male");
  });

  it("Localization works", function() {
    cy.visitContentPage(tenant, contentId);

    cy.getByCy("form-field-name")
      .closest(".v-input")
      .find(".v-label")
      .should("contain", "Name");
    cy.get(".culture-picker")
      .first()
      .click({ force: true });
    cy.getByCy("form-field-name")
      .closest(".v-input")
      .find(".v-label")
      .should("contain", "Nom");
  });

  it("Using Submit state to disable the button", function() {
    cy.visitContentPage(tenant, contentId);

    cy.getByCy("form-field-name").type("Jean-Philippe Tissot", { force: true });
    cy.getByCy("form-field-email").type("jp@email.com", { force: true });
    cy.getByCy("form-field-gender")
      .click({ force: true })
      .type("{downarrow}{downarrow}{enter}", { force: true });
    cy.getByCy("form-button-submit").click();
    cy.getByCy("form-button-submit").should("be.disabled");
    cy.getByCy("form-successmessage").contains(
      "Your form was submitted successfully."
    );
  });

  it("Clearing the form state works", function() {
    cy.visitContentPage(tenant, contentId);

    cy.getByCy("form-field-email").type("email@email.com", { force: true });
    cy.getByCy("form-field-gender")
      .click({ force: true })
      .type("{downarrow}{enter}", { force: true });

    cy.getByCy("form-button-submit").click();
    cy.getByCy("form-field-name")
      .closest(".v-input")
      .find(".v-messages__message")
      .should("contain", "The name is required");
    cy.getByCy("form-errormessage").contains(
      "An error occurred while submitting your form."
    );

    cy.getByCy("form-button-formClear").click();
    cy.getByCy("form-field-gender").should("be.empty");
    cy.getByCy("form-field-email").should("be.empty");
  });

  it("Bag fields can be added", function() {
    cy.visitContentPage(tenant, bagFormId);

    cy.getByCy("vue-form-bag-add-button").click();

    cy.get(".v-text-field__slot").should("have.length", 6); // There are two prototypes that are hidden
  });

  it("Bag fields can be removed", function() {
    cy.visitContentPage(tenant, bagFormId);

    cy.getByCy("vue-form-bag-remove-button").click();

    cy.get(".v-text-field__slot").should("have.length", 2); // There are two prototypes that are hidden
  });
});
