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
        const Models.Dal.Saber.SaberColor MY_SABER_DEFAULT_ARBITRARY_COLOR = Models.Dal.Saber.SaberColor.Blue;


        [Given(@"I have Saber")]
        public void GivenIHaveSaber()
        {
            // Create it the first time.
            ScenarioContext.Current.Add(
                MY_SABER_KEY,
                new Models.Dal.Saber() { Id = DEFAULT_SABER_ID, Color = MY_SABER_DEFAULT_ARBITRARY_COLOR });
        }

        [When(@"I choose a given color while the laser is ON")]
        public void WhenIChooseAGivenColorWhileTheLaserIsON(Table tableOfColors)
        {
            Models.Dal.Saber mySaber = (Models.Dal.Saber) ScenarioContext.Current[MY_SABER_KEY];

            foreach (var color in tableOfColors.Rows)
            {
                if (color["Is_Saber_On"]=="1")
                {
                    // Sets the wanted color.
                    mySaber.Color = GetColor(color);

                    // Turn it on. Else won't see anything.
                    //mySaber.TurnOn();

                    //Assert.AreEqual<string>(color["Given_Color"], mySaber.SaberColor.ToString());
                }
                else
                {
                    // Check that this is OFF by default.
                    //Assert.IsFalse(mySaber.isSaberOn);
                }
            }
        }

        [Then(@"the laser should be colored accordingly")]
        public void ThenTheLaserShouldBeColoredAccordingly()
        {
            // Assert already done.
        }


        private static Models.Dal.Saber.SaberColor GetColor(TableRow color)
        {
            Models.Dal.Saber.SaberColor myPossibleColor;
            // Should EnumConverter to encapsulate this.
            switch (color["Given_Color"])
            {

                case "Red": myPossibleColor = Models.Dal.Saber.SaberColor.Red; break;
                case "Blue": myPossibleColor = Models.Dal.Saber.SaberColor.Blue; break;
                case "Green": myPossibleColor = Models.Dal.Saber.SaberColor.Green; break;
                case "Violet": myPossibleColor = Models.Dal.Saber.SaberColor.Violet; break;
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
            Models.Dal.Saber mySaber = (Models.Dal.Saber) ScenarioContext.Current[MY_SABER_KEY];
            //Assert.IsTrue(!mySaber.isSaberOn);
        }
    }
}
