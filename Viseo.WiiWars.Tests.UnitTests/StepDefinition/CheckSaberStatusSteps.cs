using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TechTalk.SpecFlow;

// Ctrl-K-D to format
namespace Viseo.WiiWars.Tests.UnitTests.StepDefinition
{
    [Binding]
    public class CheckSaberStatusSteps
    {
        const int DEFAULT_SABER_ID = 1;
        const string MY_SABER_KEY = "MY_SABER_KEY";
        const Models.Saber.enumSaberColor MY_SABER_DEFAULT_ARBITRARY_COLOR = Models.Saber.enumSaberColor.Blue;


        [Given(@"I have Saber")]
        public void GivenIHaveSaber()
        {
            // Create it the first time.
            ScenarioContext.Current.Add(
                MY_SABER_KEY,
                new Models.Saber(DEFAULT_SABER_ID, MY_SABER_DEFAULT_ARBITRARY_COLOR));
        }

        [When(@"I choose a given color while the laser is ON")]
        public void WhenIChooseAGivenColorWhileTheLaserIsON(Table tableOfColors)
        {
            Models.Saber mySaber = (Models.Saber) ScenarioContext.Current[MY_SABER_KEY];

            foreach (var color in tableOfColors.Rows)
            {
                if (color["Is_Saber_On"]=="1")
                {
                    // Sets the wanted color.
                    mySaber.SaberColor = GetColor(color);

                    // Turn it on. Else won't see anything.
                    mySaber.TurnOn();

                    Assert.AreEqual<string>(color["Given_Color"], mySaber.SaberColor.ToString());
                }
                else
                {
                    // Check that this is OFF by default.
                    Assert.IsFalse(mySaber.isSaberOn);
                }
            }
        }

        [Then(@"the laser should be colored accordingly")]
        public void ThenTheLaserShouldBeColoredAccordingly()
        {
            // Assert already done.
        }


        private static Models.Saber.enumSaberColor GetColor(TableRow color)
        {
            Models.Saber.enumSaberColor myPossibleColor;
            // Should EnumConverter to encapsulate this.
            switch (color["Given_Color"])
            {

                case "Red": myPossibleColor = Models.Saber.enumSaberColor.Red; break;
                case "Blue": myPossibleColor = Models.Saber.enumSaberColor.Blue; break;
                case "Green": myPossibleColor = Models.Saber.enumSaberColor.Green; break;
                case "Violet": myPossibleColor = Models.Saber.enumSaberColor.Violet; break;
                default: myPossibleColor = 0; break;
            }

            return myPossibleColor;
        }
        
        [When(@"first have it")]
        public void WhenFirstHaveIt()
        {
            // Nothing here.
        }

        [Then(@"the Saber should Off")]
        public void ThenTheSaberShouldOff()
        {
            // Use it and check it is OFF.
            Models.Saber mySaber = (Models.Saber) ScenarioContext.Current[MY_SABER_KEY];
            Assert.IsTrue(!mySaber.isSaberOn);
        }
    }
}
