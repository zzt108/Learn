using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace LearnBdd.Specs
{
  [Binding]
  public class BddBasicsSteps
  {
    private Dictionary<string, int> data = new Dictionary<string, int>();

    [Given(@"there is a question ""(.*)"" with the answers")]
    public void GivenThereIsAQuestionWithTheAnswers(string p0, Table table)
    {
      Console.WriteLine("Question {0}", p0);
      foreach (var row in table.Rows)
      {
        var v = row.Values.ToArray();
        Console.WriteLine("Row {0} {1}", v[0], v[1]);
        data.Add(v[0], int.Parse(v[1]));
      }
    }

    [When(@"you upvote answer ""(.*)""")]
    public void WhenYouUpvoteAnswer(string p0)
    {
      data[p0]++;
    }

    [Then(@"the answer ""(.*)"" should be on top")]
    public void ThenTheAnswerShouldBeOnTop(string p0)
    {
      Assert.AreEqual(2,data[p0],"data not incremented");  
    }
  }
}
