using System;
using TechTalk.SpecFlow;

namespace LearnBdd.Specs
{
  [Binding]
  public class BddBasicsSteps
  {
    [Given(@"there is a question ""(.*)"" with the answers")]
    public void GivenThereIsAQuestionWithTheAnswers(string p0, Table table)
    {
      Console.WriteLine("Question {0}", p0);
      foreach (var row in table.Rows)
      {
        Console.WriteLine("Row {0}", row.ToString());
      }
      //ScenarioContext.Current.Pending();
    }

    [When(@"you upvote answer ""(.*)""")]
    public void WhenYouUpvoteAnswer(string p0)
    {
      ScenarioContext.Current.Pending();
    }

    [Then(@"the answer ""(.*)"" should be on top")]
    public void ThenTheAnswerShouldBeOnTop(string p0)
    {
      ScenarioContext.Current.Pending();
    }
  }
}
