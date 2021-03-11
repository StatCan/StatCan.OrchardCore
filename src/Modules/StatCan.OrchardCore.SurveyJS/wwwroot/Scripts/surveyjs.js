function initSurveyJS(surveyjs) {
  var decodedJson = decodeURIComponent(surveyjs.dataset.data).split('+').join('')
  var id = surveyjs.dataset.id

    Survey
        .StylesManager
        .applyTheme("default");

    Survey
        .JsonObject
        .metaData
        .addProperty("itemvalue", {name: "score:number"});
    
    window.survey = new Survey.Model(decodedJson);
  
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
          var score = JSON.stringify(totalScore)

          if(score != "null") {
            document
              .querySelector('.surveyResult')
              .innerHTML = "Total Score is: " + score;
          } 
        });
      
    survey.render(id+'-result');
} 

document.querySelectorAll('.surveyResult').forEach(initSurveyJS);
