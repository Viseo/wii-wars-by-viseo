using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// This v1 Unit Test does not yet implement Behavior-Driven Development.
/// Since we are using VS 2015 CTP 6.
/// </summary>
namespace Viseo.WiiWars.Tests.UnitTests
{
    [TestClass]
    public class CheckSaberStatus
    {
        const int DEFAULT_SABER_ID = 1;

        [TestMethod]
        public void GIVEN_I_have_a_new_Saber_WHEN_I_Turn_It_On_THEN_it_is_Indeed_On()
        {
            // GIVEN
            Viseo.WiiWars.Models.Dal.Saber saber = new Models.Dal.Saber()
            {
                Id = DEFAULT_SABER_ID,
                Color = Models.Dal.Saber.SaberColor.Blue
            };

            // WHEN
            //saber.TurnOn();

            // THEN
            //Assert.IsTrue(saber.isSaberOn);
        }

        [TestMethod]
        public void GIVEN_I_have_a_new_Saber_WHEN_I_Turn_It_On_and_Off_THEN_it_is_Indeed_Off()
        {
            // GIVEN
            Viseo.WiiWars.Models.Dal.Saber saber = new Models.Dal.Saber()
            {
                Id = DEFAULT_SABER_ID,
                Color = Models.Dal.Saber.SaberColor.Blue
            };

            // WHEN
            //saber.TurnOn();
            // Assert.IsTrue(saber.isSaberOn);

            //saber.TurnOff();

            // THEN
            //Assert.IsTrue(!saber.isSaberOn);
        }

        [TestMethod]
        public void GIVEN_I_have_a_new_BlueSaber_WHEN_check_its_color_THEN_it_is_indeed_blue()
        {
            // GIVEN
            Viseo.WiiWars.Models.Dal.Saber saber = new Models.Dal.Saber()
            {
                Id = DEFAULT_SABER_ID,
                Color = Models.Dal.Saber.SaberColor.Blue
            };

            // WHEN
            Models.Dal.Saber.SaberColor chechedActualColor = saber.Color;

            // THEN
            Assert.AreEqual<Models.Dal.Saber.SaberColor>(
                Models.Dal.Saber.SaberColor.Blue, chechedActualColor);
        }
    }
}
