/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

const contentIdChallengeSubmissionForm = "4kpgcnkaydrkb5hfxy10261qmb";
const contentIdHackerRegistrationForm = "4dj03mdaztzf8wg12gw56xkg43";
const contentIdVolunteerRegistrationForm = "4jjje1q162zaay11nbf3w2634h";
const contentIdScoringPageForm = "423fn7nvrwcdv2ksy8hww9qwdn";

describe("Hackathon Tests", function() {    
  let tenant;

  before(() => {
    tenant = generateTenantInfo("hackathon-theme-setup")
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.uploadRecipeJson(tenant, "recipes/users.json");
  })
  
  it("Can login to Hackathon site", function() {
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.enableFeature(tenant, "StatCan_OrchardCore_Hackathon");
    cy.enableFeature(tenant, "StatCan_OrchardCore_Hackathon_Team");
    cy.enableFeature(tenant, "StatCan_OrchardCore_Hackathon_Judging");
  })

  it("Can run Hackathon recipes recipes", function() {
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.runRecipe(tenant, 'Hackathon_ChallengeSubmissionForm');
    cy.runRecipe(tenant, 'Hackathon_HackerRegistrationForm');
   // cy.runRecipe(tenant, 'Hackathon_ScoringPage');
    cy.runRecipe(tenant, 'Hackathon_VolunteerRegistrationForm');
  })

  // Challenge Submission Form
  it("Challenge Submission: Client side validation prevents submit", function() {
    cy.loginAs(tenant.prefix, 'Challenger', 'Inno111!');
    cy.visitContentPage(tenant, contentIdChallengeSubmissionForm);

    cy.get('button[type=submit]').click();

    cy.get('input[name=name]').closest('.v-input').find('.v-messages__message').should('contain', 'The name field is required');
    cy.get('input[name=shortDescription]').closest('.v-input').find('.v-messages__message').should('contain', 'The shortDescription field is required');
    cy.get('textarea[name=description]').closest('.v-input').find('.v-messages__message').should('contain', 'The description field is required');
  })

  it("Challenge Submission: Submit works and displays success message", function() {
    cy.loginAs(tenant.prefix, 'Challenger', 'Inno111!');
    cy.visitContentPage(tenant, contentIdChallengeSubmissionForm);

    cy.get('input[name=name]').type('Challenge Name', {force:true});
    cy.get('input[name=shortDescription]').type('Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.', {force:true});
    cy.get('textarea[name=description]').type('Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.', {force:true});

    cy.get('button[type=submit]').click();

    cy.get('div[class=v-alert__content]').should('contain', 'Your challenge was submitted successfully');
  })

  // Hacker Registration Form
  it("Hacker Registration: Client side validation prevents submit", function() {
    cy.loginAs(tenant.prefix, 'Hacker', 'Inno111!');
    cy.visitContentPage(tenant, contentIdHackerRegistrationForm);
  
    cy.get('button[type=submit]').click();
  
    cy.get('input[name=firstName]').closest('.v-input').find('.v-messages__message').should('contain', 'The First name field is required');
    cy.get('input[name=lastName]').closest('.v-input').find('.v-messages__message').should('contain', 'The Last name field is required');
    cy.get('input[name=email]').closest('.v-input').find('.v-messages__message').should('contain', 'The Contact Email Address field is required');
  })
  
  it("Hacker Registration: Submit works and displays success message", function() {
    cy.loginAs(tenant.prefix, 'James', 'Inno111!');
    cy.visitContentPage(tenant, contentIdHackerRegistrationForm);
  
    cy.get('input[name=firstName]').type('Tester', {force:true});
    cy.get('input[name=lastName]').type('McTest', {force:true});
    cy.get('input[name=email]').type('test@testing.com', {force:true});
    cy.get('input[value=en]').click({force:true});
  
    cy.get('button[type=submit]').click();
    
    cy.get('div[class=v-alert__content]').should('contain', 'You have successfully registered!');
  })
    
  // Volunteer Registration Form
  it("Volunteer Registration: Client side validation prevents submit", function() {
    cy.loginAs(tenant.prefix, 'Volunteer', 'Inno111!');
    cy.visitContentPage(tenant, contentIdVolunteerRegistrationForm);
  
    cy.get('button[type=submit]').click();
  
    cy.get('input[name=firstName]').closest('.v-input').find('.v-messages__message').should('contain', 'The First name field is required');
    cy.get('input[name=lastName]').closest('.v-input').find('.v-messages__message').should('contain', 'The Last name field is required');
    cy.get('input[name=email]').closest('.v-input').find('.v-messages__message').should('contain', 'The Email Address field is required');
  })
  
  it("Volunteer Registration: Submit works and displays success message", function() {
    cy.loginAs(tenant.prefix, 'Alan', 'Inno111!');
    cy.visitContentPage(tenant, contentIdVolunteerRegistrationForm);
  
    cy.get('input[name=firstName]').type('Tester', {force:true});
    cy.get('input[name=lastName]').type('McTest', {force:true});
    cy.get('input[name=email]').type('test@testing.com', {force:true});
    cy.get('input[value=en]').click({force:true});
  
    cy.get('button[type=submit]').click();
    
    cy.get('div[class=v-alert__content]').should('contain', 'You have successfully registered!');
  })

  // // Scoring Page Form  
  // it("Submit works and displays success message", function() {
  //   cy.visitContentPage(tenant, contentIdScoringPageForm);
  
  //   cy.get('input[value=7]').click({force:true});
  //   cy.get('textarea[name=comment]').type('really good blah blah blah blah blah blah blah blah blah', {force:true});
  
  //   cy.get('button[type=submit]').click();
    
  //   cy.get('div[class=v-alert__content]').should('contain', 'Your submission was successful');
  // })
});
