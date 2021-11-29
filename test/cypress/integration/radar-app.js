/// <reference types="Cypress" />
import { generateTenantInfo } from "cypress-orchardcore/dist/utils";

let tenant;

describe("Radar app tests", function() {
  it("Radar app setup is successful", function() {
    tenant = generateTenantInfo("radar-setup", "Radar app recipe");
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.uploadRecipeJson(tenant, "recipes/radar-test.json");
  });

  it("Topic can be created", function() {
    cy.login(tenant);
    createTopic("Orchard Core");
  });

  it("Topic can be updated", function() {
    cy.login(tenant);
    createTopic("Java");
    updateTopic("C#");
  });

  it("Project can be created", function() {
    cy.login(tenant);
    createProject("Project 1");
  });

  it("Project can be updated", function() {
    cy.login(tenant);
    createProject("Project 2");
    updateProject("Project 3");
  });

  it("Proposal can be created", function() {
    cy.login(tenant);
    createProposal("Proposal 1");
  });

  it("Proposal can be updated", function() {
    cy.login(tenant);
    createProposal("Proposal 2");
    updateProposal("Proposal 3");
  });

  it("Community can be created", function() {
    cy.login(tenant);
    createCommunity("Community 1");
  });

  it("Community can be updated", function() {
    cy.login(tenant);
    createCommunity("Community 2");
    updateCommunity("Community 3");
  });

  it("Event can be created", function() {
    cy.login(tenant);
    createEvent("Event 1");
  });

  it("Event can be updated", function() {
    cy.login(tenant);
    createEvent("Event 2");
    updateEvent("Event 3");
  });

  it("Artifact can be created", function() {
    cy.login(tenant);
    createArtifact("Artifact 1", "https://www.github.com", "Project 4");
  });

  it("Artifact can be updated", function() {
    cy.login(tenant);
    createArtifact("Artifact 2", "https://www.github.com", "Project 5");
    updateArtifact("Artifact 3", "https://www.github.com");
  });
});

function createArtifact(name, link, projectName) {
  cy.visit(`${tenant.prefix}/projects/create`);

  createProject(projectName);

  cy.get(".workspace-add-button").click();

  // Name
  cy.get(".v-text-field__slot")
    .eq(0)
    .type(name);

  // Url
  cy.get(".v-text-field__slot")
    .eq(1)
    .type(link);

  // Roles
  cy.get(".multiselect__input")
    .eq(1)
    .type("Administrator{enter}", { force: true });

  // Visibility
  cy.get(".multiselect__input")
    .eq(0)
    .type("Publish{enter}", { force: true });

  cy.get("[type='submit']").click();

  cy.contains(name).should("be.visible");
}

function updateArtifact(name) {
  cy.get(".options-button").click();
  cy.get(".options-button-update-link").click();

  // Name
  cy.get(".v-text-field__slot > input")
    .eq(0)
    .clear()
    .type(name);

  cy.get("[type='submit']").click();

  cy.contains(name).should("be.visible");
}

function createEvent(name) {
  cy.visit(`${tenant.prefix}/events/create`);

  // Name
  cy.get(".v-text-field__slot")
    .eq(0)
    .type(name);

  // Description
  cy.get(".v-text-field__slot")
    .eq(1)
    .type(name);

  // Roles
  cy.get(".multiselect__input")
    .eq(3)
    .type("Administrator{enter}", { force: true });

  // Visibility
  cy.get(".multiselect__input")
    .eq(4)
    .type("Publish{enter}", { force: true });

  cy.get("[type='submit']").click();

  cy.contains(name).should("be.visible");
}

function updateEvent(name) {
  cy.get(".options-button").click();
  cy.get(".options-button-update-link").click();

  // Name
  cy.get(".v-text-field__slot > input")
    .eq(0)
    .clear()
    .type(name);

  cy.get("[type='submit']").click();

  cy.contains(name).should("be.visible");
}

function createProject(name) {
  cy.visit(`${tenant.prefix}/projects/create`);

  // Name
  cy.get(".v-text-field__slot")
    .eq(0)
    .type(name);

  // Description
  cy.get(".v-text-field__slot")
    .eq(1)
    .type(name);

  // Type
  cy.get(".multiselect__input")
    .eq(0)
    .type("ProjectType{enter}", { force: true });

  // Roles
  cy.get(".multiselect__input")
    .eq(2)
    .type("Administrator{enter}", { force: true });

  // Visibility
  cy.get(".multiselect__input")
    .eq(3)
    .type("Publish{enter}", { force: true });

  cy.get("[type='submit']").click();

  cy.contains(name).should("be.visible");
}

function updateProject(name) {
  cy.get(".options-button").click();
  cy.get(".options-button-update-link").click();

  // Name
  cy.get(".v-text-field__slot > input")
    .eq(0)
    .clear()
    .type(name);

  cy.get("[type='submit']").click();

  cy.contains(name).should("be.visible");
}

function createProposal(name) {
  cy.visit(`${tenant.prefix}/proposals/create`);

  // Name
  cy.get(".v-text-field__slot")
    .eq(0)
    .type(name);

  // Description
  cy.get(".v-text-field__slot")
    .eq(1)
    .type(name);

  // Type
  cy.get(".multiselect__input")
    .eq(0)
    .type("ProposalType{enter}", { force: true });

  // Roles
  cy.get(".multiselect__input")
    .eq(3)
    .type("Administrator{enter}", { force: true });

  // Visibility
  cy.get(".multiselect__input")
    .eq(4)
    .type("Publish{enter}", { force: true });

  cy.get("[type='submit']").click();

  cy.contains(name).should("be.visible");
}

function updateProposal(name) {
  cy.get(".options-button").click();
  cy.get(".options-button-update-link").click();

  // Name
  cy.get(".v-text-field__slot > input")
    .eq(0)
    .clear()
    .type(name);

  cy.get("[type='submit']").click();

  cy.contains(name).should("be.visible");
}

function createCommunity(name) {
  cy.visit(`${tenant.prefix}/communities/create`);

  // Name
  cy.get(".v-text-field__slot")
    .eq(0)
    .type(name);

  // Description
  cy.get(".v-text-field__slot")
    .eq(1)
    .type(name);

  // Type
  cy.get(".multiselect__input")
    .eq(0)
    .type("CommunityType{enter}", { force: true });

  // Roles
  cy.get(".multiselect__input")
    .eq(3)
    .type("Administrator{enter}", { force: true });

  // Visibility
  cy.get(".multiselect__input")
    .eq(4)
    .type("Publish{enter}", { force: true });

  cy.get("[type='submit']").click();

  cy.contains(name).should("be.visible");
}

function updateCommunity(name) {
  cy.get(".options-button").click();
  cy.get(".options-button-update-link").click();

  // Name
  cy.get(".v-text-field__slot > input")
    .eq(0)
    .clear()
    .type(name);

  cy.get("[type='submit']").click();

  cy.contains(name).should("be.visible");
}

function createTopic(name) {
  cy.visit(`${tenant.prefix}/topics/create`);

  // Name
  cy.get(".v-text-field__slot")
    .eq(0)
    .type(name);

  // Description
  cy.get(".v-text-field__slot")
    .eq(1)
    .type(name);
  // Roles
  cy.get(".multiselect__input").type("Administrator{enter}", { force: true });

  cy.get("[type='submit']").click();

  cy.contains(name).should("be.visible");
}

function updateTopic(name) {
  cy.get(".options-button").click();
  cy.get(".options-button-update-link").click();

  // Name
  cy.get(".v-text-field__slot > input")
    .eq(0)
    .clear()
    .type(name);

  cy.get("[type='submit']").click();

  cy.contains(name).should("be.visible");
}
