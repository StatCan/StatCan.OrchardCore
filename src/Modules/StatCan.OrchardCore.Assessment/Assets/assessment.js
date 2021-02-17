function initAssessment(assessment) {
  var json = assessment.dataset.data
  var id = assessment.dataset.id

    Survey
        .StylesManager
        .applyTheme("default");

    Survey
        .JsonObject
        .metaData
        .addProperty("itemvalue", {name: "score:number"});
    
    window.survey = new Survey.Model(json);
  
    survey
        .onComplete
        .add(function (result) {

          var plainData = result.getPlainData({
            calculations: [{ propertyName: "score"}]
          });
          var totalScore = 0
          function calcScore(data) {
            return (data || [] ).reduce(function(sum, item) {
              return sum+ (item.isNode ? calcScore(item.data) : item.score);
              }, 0)
          }
          
          totalScore = calcScore(plainData);

          document
            .querySelector('.surveyResult')
            .innerHTML = "Total Score is: " + JSON.stringify(totalScore);
        });
      
    survey.render(id);
} 

document.querySelectorAll('.surveyResult').forEach(initAssessment);
