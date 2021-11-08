/// <reference types="Cypress" />
import { generateTenantInfo } from 'cypress-orchardcore/dist/utils';

const contentIdChallengeSubmissionForm = "4kpgcnkaydrkb5hfxy10261qmb";
const contentIdHackerRegistrationForm = "4dj03mdaztzf8wg12gw56xkg43";
const contentIdVolunteerRegistrationForm = "4jjje1q162zaay11nbf3w2634h";
const contentIdConfirmPresenceForm = "4qxg2sr9b4ywatnhjkeq90qbcv";
const contentIdNewsletterForm = "4ty56s2p8vfhmxp76p4f37b1c3";
const contentIdScoringForm = "423fn7nvrwcdv2ksy8hww9qwdn";
const contentIdSolutionSubmissionForm = "4fy9a011te6qx5611jv1hhtwwh";
const contentIdUserProfileForm = "4pbmvhq49bwy1yatrapasxctnk";
const scoringTeamId = "46p8s03jb80m037f96frfhs9mv";
const TeamManagementPagePath = "test-page";
const draggableContentId = "45a6j8rkq9z56xfhvxwm5hvgc7";
const password = 'Inno111!';

describe("Candev Tests", function() {    
  let tenant;
  let ContentIdTeam;

  before(() => {
    tenant = generateTenantInfo("candev-app")
    cy.newTenant(tenant);
    cy.login(tenant);
    cy.uploadRecipeJson(tenant, "recipes/users.json");
  })
  
  it("Can login to Candev site", function() {
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.uploadRecipeJson(tenant, "recipes/candev-content.json");
  })

  it("Can run Candev recipes", function() {
    cy.visit(`${tenant.prefix}/login`)
    cy.login(tenant);
    cy.runRecipe(tenant, 'Candev_ChallengeSubmissionForm');
    cy.runRecipe(tenant, 'Candev_HackerRegistrationForm');
    cy.runRecipe(tenant, 'Candev_VolunteerRegistrationForm');
    cy.runRecipe(tenant, 'Candev_TeamEdit');
    cy.runRecipe(tenant, 'Candev_ConfirmPresenceForm');
    cy.runRecipe(tenant, 'Candev_Newsletter');
    cy.runRecipe(tenant, 'Candev_ScoringPage');
    cy.runRecipe(tenant, 'Candev_SolutionForm');
    cy.runRecipe(tenant, 'Candev_UserProfile');
  })

  //#region Challenge Submissiion form
  it("Challenge Submission: Client side validation prevents submit", function() {
    cy.login(tenant);
    cy.visitContentPage(tenant, contentIdChallengeSubmissionForm);

    cy.get('button[type=submit]').click();

    cy.get('input[name=title]').closest('.v-input').find('.v-messages__message').should('contain', 'The title field is required');
  })

  it("Challenge Submission: Submit works and displays success message", function() {
    cy.login(tenant);
    cy.visitContentPage(tenant, contentIdChallengeSubmissionForm);

    cy.get('input[name=title]').type('Title', {force:true});
    cy.get('button[type=button]').eq(2).click({force:true});
    cy.get('textarea[name=statement]').type('Statement', {force:true});
    cy.get('button[type=button]').eq(3).click({force:true});
    cy.get('textarea[name=datasets]').type('Datasets', {force:true});
    cy.get('button[type=button]').eq(4).click({force:true});
    cy.get('textarea[name=backgroundInfo]').type('Background Info', {force:true});
    cy.get('button[type=button]').eq(5).click({force:true});
    cy.get('input[name=keywords]').type('Keywords', {force:true});
    cy.get('button[type=button]').eq(6).click({force:true});
    cy.get('input[name=organizationNameEn]').type('Organization Name', {force:true});
    cy.get('input[name=organizationAcronymEn]').type('Organization Acronym', {force:true});
    cy.get('button[type=button]').eq(7).click({force:true});
    cy.get('input[name=contactPersonName]').type('Contact Person Name', {force:true});
    cy.get('input[name=contactPersonEmail]').type('test@testing.com', {force:true});
    cy.get('input[name=technicalMentorName]').type('Technical Mentor Name', {force:true});
    cy.get('input[name=technicalMentorEmail]').type('test@testing.com', {force:true});
    cy.get('input[name=caseSpecialistMentorName]').type('Case Specialist Name', {force:true});
    cy.get('input[name=caseSpecialistMentorEmail]').type('test@testing.com', {force:true});
    cy.get('button[type=button]').eq(8).click({force:true});
    cy.get('textarea[name=comments]').type('Comments', {force:true});

    cy.get('button[type=submit]').click();

    cy.get('div[class=v-alert__content]').should('contain', 'Your challenge was submitted successfully');
  })
  //#endregion

  //#region Hacker Registration Form
  it("Hacker Registration: Requires login", function() {
    cy.visitContentPage(tenant, contentIdHackerRegistrationForm);
    cy.get("#main").contains("Please login before registering to this event");
  })

  it("Hacker Registration: Client side validation prevents submit", function() {
    cy.loginAs(tenant.prefix, 'Hacker', password);
    cy.visitContentPage(tenant, contentIdHackerRegistrationForm);
  
    cy.get('button[type=submit]').click();
  
    cy.get('input[name=firstName]').closest('.v-input').find('.v-messages__message').should('contain', 'The First name field is required');
    cy.get('input[name=lastName]').closest('.v-input').find('.v-messages__message').should('contain', 'The Last name field is required');
    cy.get('input[name=email]').closest('.v-input').find('.v-messages__message').should('contain', 'The Contact email address field is required');
    cy.get('input[name=school]').closest('.v-input').find('.v-messages__message').should('contain', 'The University or college name field is required');
    cy.get('input[name=fieldOfStudy]').closest('.v-input').find('.v-messages__message').should('contain', 'The Field of study field is required');
    cy.get('input[name=programName]').closest('.v-input').find('.v-messages__message').should('contain', 'The Program name field is required');
    cy.get('input[name=programLevel]').closest('.v-input').find('.v-messages__message').should('contain', 'The Program level field is required');
    cy.get('input[name=programYears]').closest('.v-input').find('.v-messages__message').should('contain', 'The Number of years completed in your program field is required');
    cy.get('input[name=adult]').closest('.v-input').find('.v-messages__message').should('contain', 'The I am 18 years or older field is required');
    cy.get('input[name=termsAndConditions]').closest('.v-input').find('.v-messages__message').should('contain', 'The I have read and I agree to the terms and conditions field is required');
  })
  
  it("Hacker Registration: Submit works and displays success message", function() {
    cy.loginAs(tenant.prefix, 'James', password);
    cy.visitContentPage(tenant, contentIdHackerRegistrationForm);
  
    cy.get('input[name=firstName]').type('Tester', {force:true});
    cy.get('input[name=lastName]').type('McTest', {force:true});
    cy.get('input[name=email]').type('test@testing.com', {force:true});
    cy.get('input[value=en]').click({force:true});
    cy.get('input[name=school]').type('school name', {force:true});
    cy.get('input[name=fieldOfStudy]').type('field of study', {force:true});
    cy.get('input[name=programName]').type('program name', {force:true});
    cy.get('input[name=programLevel]').click({force:true}).get('div[id=list-item-92-0]').click({force:true});
    cy.get('input[name=programYears]').click({force:true}).get('div[id=list-item-104-0]').click({force:true});
    cy.get('input[name=adult]').click({force:true});
    cy.get('input[name=termsAndConditions]').click({force:true});
  
    cy.get('button[type=submit]').click();
    
    cy.get('div[class=v-alert__content]').should('contain', 'You have successfully registered!');
  })
  //#endregion  

  //#region Volunteer Registration Form
  it("Volunteer Registration: Client side validation prevents submit", function() {
    cy.loginAs(tenant.prefix, 'Volunteer', password);
    cy.visitContentPage(tenant, contentIdVolunteerRegistrationForm);
  
    cy.get('button[type=submit]').click();
  
    cy.get('input[name=firstName]').closest('.v-input').find('.v-messages__message').should('contain', 'The First name field is required');
    cy.get('input[name=lastName]').closest('.v-input').find('.v-messages__message').should('contain', 'The Last name field is required');
    cy.get('input[name=email]').closest('.v-input').find('.v-messages__message').should('contain', 'The Email address field is required');
    cy.get('input[name=department]').closest('.v-input').find('.v-messages__message').should('contain', 'The Department/Agency field is required');
    cy.get('input[name=termsAndConditions]').closest('.v-input').find('.v-messages__message').should('contain', 'The I have read and I agree to the terms and conditions. field is required');
  })
  
  it("Volunteer Registration: Submit works and displays success message", function() {
    cy.loginAs(tenant.prefix, 'Alan', password);
    cy.visitContentPage(tenant, contentIdVolunteerRegistrationForm);
  
    cy.get('input[name=firstName]').type('Tester', {force:true});
    cy.get('input[name=lastName]').type('McTest', {force:true});
    cy.get('input[name=email]').type('test@testing.com', {force:true});
    cy.get('input[name=mentor]').click({force:true});
    cy.get('input[value=en]').click({force:true});
    cy.get('input[name=department]').type('Department', {force:true});
    cy.get('input[name=termsAndConditions]').click({force:true});
    cy.get('input[name=comments]').type('Comments', {force:true});

    cy.get('button[type=submit]').click();
    
    cy.get('div[class=v-alert__content]').should('contain', 'You have successfully registered!');
  })
  //#endregion

  //#region Team Dashboard
  it("Team Dashboard: Create Team", function() {
    cy.loginAs(tenant.prefix, 'Laura', password);
    cy.visit(`${tenant.prefix}/${TeamManagementPagePath}`)
  
    cy.get('button[name=btnCreate]').click();

    cy.get('div[class=v-alert__content]').should('contain', 'Successfully created team');
    cy.get('span[id=team-id]').should(($span) => ContentIdTeam = $span.text());
  })

  it("Team Dashboard: Join Team", function() {
    // Mike joins the team then gets removed by the captain
    cy.loginAs(tenant.prefix, 'Mike', password);
    cy.visit(`${tenant.prefix}/${TeamManagementPagePath}`)
  
    cy.get('input[name=teamContentItemId]').type(ContentIdTeam, {force:true})
    cy.get('button[id=team-submit-addon]').click();

    cy.get('div[class=v-alert__content]').should('contain', 'Successfully joined team');

    // Frank joins team then leaves it
    cy.loginAs(tenant.prefix, 'Frank', password);
    cy.visit(`${tenant.prefix}/${TeamManagementPagePath}`)
  
    cy.get('input[name=teamContentItemId]').type(ContentIdTeam, {force:true})
    cy.get('button[id=team-submit-addon]').click();

    cy.get('div[class=v-alert__content]').should('contain', 'Successfully joined team');
  })

  it("Team Dashboard: Leave Team", function() {
    cy.loginAs(tenant.prefix, 'Frank', password);
    cy.visit(`${tenant.prefix}/${TeamManagementPagePath}`)
    
    cy.get('button[name=btnLeaveModal]').click();
    cy.get('button[name=btnLeave]').click();

    cy.get('div[class=v-alert__content]').should('contain', 'Successfully left team');
  })

  it("Team Dashboard: Team Captain Removes Member", function() {
    cy.loginAs(tenant.prefix, 'Laura', password);
    cy.visit(`${tenant.prefix}/${TeamManagementPagePath}`)
  
    cy.get('button[name=btnRemove]').last().click();

    cy.get('div[class=v-alert__content]').should('contain', 'Member successfully removed from the team');
  })

  it("Team Dashboard: Team Captain Edits Team", function() {
    cy.loginAs(tenant.prefix, 'Laura', password);
    cy.visit(`${tenant.prefix}/${TeamManagementPagePath}`)
  
    cy.get('input[name=teamName]').type('Better Team', {force:true});
    cy.get('input[name=teamDescription]').type('Lorem ipsum dolor sit amet, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.', {force:true});

    cy.get('button[name=btnSave]').click();

    cy.get('div[class=v-alert__content]').should('contain', 'Team updated successfully');
  })
  //#endregion

  //#region Confirm Presence Form
  it("Confirm Presence Form: Client side validation prevents submit", function() {
    cy.login(tenant);
    cy.visitContentPage(tenant, contentIdConfirmPresenceForm);
  
    cy.get('button[type=submit]').click();
    
    cy.get('div[class=v-alert__content]').should('contain', 'An error occurred during submission');
  })

  it("Confirm Presence Form: Submit works and displays success message", function() {
    cy.login(tenant);
    cy.visitContentPage(tenant, contentIdConfirmPresenceForm);
  
    cy.get('input[value=true]').click({force:true});
  
    cy.get('button[type=submit]').click();
    
    cy.get('div[class=v-alert__content]').should('contain', 'Your presence has been confirmed!');
  })
  //#endregion

  //#region Newsletter Form
  it("Newsletter Form: Client side validation prevents submit", function() {
    cy.login(tenant);
    cy.visitContentPage(tenant, contentIdNewsletterForm);
  
    cy.get('button[type=submit]').click();
  
    cy.get('input[name=email]').closest('.v-input').find('.v-messages__message').should('contain', 'The email address field is required');
  })
  
  it("Newsletter Form: Submit works and displays success message", function() {
    cy.login(tenant);
    cy.visitContentPage(tenant, contentIdNewsletterForm);
  
    cy.get('input[name=email]').type('test@testing.com', {force:true});
  
    cy.get('button[type=submit]').click();
    
    cy.get('div[class=v-alert__content]').should('contain', 'Thank you, you will be kept up to date with news about the event!');
  })
  //#endregion

  //#region Scoring Form
  it("Scoring Form: Client side validation prevents submit", function() {
    cy.loginAs(tenant.prefix, 'Adam', password);
    cy.visit(`${tenant.prefix}/Contents/ContentItems/${contentIdScoringForm}?teamId=${scoringTeamId}`)
  
    cy.get('button[type=submit]').click();
  
    cy.get('div[class=v-alert__content]').should('contain', 'A score is required.');
  })
  
  it("Scoring Form: Submit works and displays success message", function() {
    cy.loginAs(tenant.prefix, 'Adam', password);
    cy.visit(`${tenant.prefix}/Contents/ContentItems/${contentIdScoringForm}?teamId=${scoringTeamId}`)
  
    cy.get('input[value=4]').click({force:true});
  
    cy.get('button[type=submit]').click();
    
    cy.get('div[class=v-alert__content]').should('contain', 'Your submission was successful');
  })
  //#endregion

  //#region Solution Submission Form
  it("Solution Submission Form: Client side validation prevents submit", function() {
    cy.login(tenant);
    cy.visitContentPage(tenant, contentIdSolutionSubmissionForm);
  
    cy.get('button[type=submit]').click();
  
    cy.get('input[name=solutionName]').closest('.v-input').find('.v-messages__message').should('contain', 'The Solution Name field is required');
    cy.get('textarea[name=solutionDescription]').closest('.v-input').find('.v-messages__message').should('contain', 'The Description field is required');
    cy.get('input[name=repositoryUrl]').closest('.v-input').find('.v-messages__message').should('contain', 'The Solution repository URL field is required');
  })
  
  it("Solution Submission Form: Submit works and displays success message", function() {
    cy.loginAs(tenant.prefix, 'Frank', password);
    cy.visit(`${tenant.prefix}/${TeamManagementPagePath}`)
  
    cy.get('button[name=btnCreate]').click();

    cy.get('div[class=v-alert__content]').should('contain', 'Successfully created team');

    cy.visitContentPage(tenant, contentIdSolutionSubmissionForm);
  
    cy.get('input[name=solutionName]').type('Solution Name', {force:true});
    cy.get('textarea[name=solutionDescription]').type('Solution Description', {force:true});
    cy.get('input[name=repositoryUrl]').type('www.soultionURL.com', {force:true});
  
    cy.get('button[type=submit]').click();
    
    cy.get('div[class=v-alert__content]').should('contain', 'Your solution was submitted successfully!');
  })
  //#endregion

  //#region User Profile Form
  it("User Profile Form: Client side validation prevents submit", function() {
    cy.login(tenant);
    cy.visitContentPage(tenant, contentIdUserProfileForm);
  
    cy.get('button[type=submit]').click();
  
    cy.get('input[name=firstName]').closest('.v-input').find('.v-messages__message').should('contain', 'The First name field is required');
    cy.get('input[name=lastName]').closest('.v-input').find('.v-messages__message').should('contain', 'The Last name field is required');
    cy.get('input[name=email]').closest('.v-input').find('.v-messages__message').should('contain', 'The email address field is required');
  })
  
  it("Solution Submission Form: Submit works and displays success message", function() {
    cy.login(tenant);
    cy.visitContentPage(tenant, contentIdUserProfileForm);
  
    cy.get('input[name=firstName]').type('FirstName', {force:true});
    cy.get('input[name=lastName]').type('LastName', {force:true});
    cy.get('input[name=email]').type('email@email.com', {force:true});
    cy.get('input[value=en]').click({force:true});
  
    cy.get('button[type=submit]').click();
    
    cy.get('div[class=v-alert__content]').should('contain', 'Your profile was updated.');
  })
  //#endregion

  it("Vue Component: Draggable list", function() {
    cy.login(tenant);
    cy.uploadRecipeJson(tenant, "recipes/draggable.json");
    cy.visitContentPage(tenant, draggableContentId);

    cy.get('div[name=title1]').should('contain', 'Title 1');
    cy.get('div[name=hint1]').should('contain', 'Hint 1');
    cy.get('span[id=error]').should('contain', 'Please select at least 3 options');

    cy.get('div[name=title').should('contain', 'Title 1');
    cy.get('div[name=hint]').should('contain', 'Hint 1');
  })
});
