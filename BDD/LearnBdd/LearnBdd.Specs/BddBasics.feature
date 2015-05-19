Feature: BddBasics
	In order to find out how Bdd works
	As a QA Engineer
	I want to see BDD basic working

@mytag
Scenario: The answer with the highest vote gets to the top
	Given there is a question "What`s your favorite colour?" with the answers
		| Answer         | Vote |
		| Red            | 1    |
		| Cucumber green | 1    | 
	When you upvote answer "Cucumber green"
	Then the answer "Cucumber green" should be on top
