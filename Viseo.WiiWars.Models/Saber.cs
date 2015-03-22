using System;

namespace Viseo.WiiWars.Models
{
    public class Saber
    {
        /// <summary>
        /// Determines the unique ID of the Saber in the world.
        /// </summary>
        private int _id;

        /// <summary>
        /// Read only property to get the GUID of the Saber.
        /// </summary>
        public int id
        {
            get { return _id; }
        }

        public enum enumSaberColor
        {
            Blue,
            Green,
            Red,
            Violet
        };

        private enumSaberColor _enumSaberColor;

        /// <summary>
        /// Allows to turn On or Off the Saber.
        /// </summary>
        private bool _isSaberOn;

        /// <summary>
        /// Read only property to check is the Saber is on or not.
        /// </summary>
        public bool isSaberOn
        {
            get { return _isSaberOn; }
        }

        public Saber()
        {

        }
        public Saber(int id)
        {
            _id = id;
        }
        public Saber(int id, enumSaberColor mySaberColor)
        {
            _id = id;
            _enumSaberColor = mySaberColor;
        }

        public void TurnOn()
        {
            switchOnOffStatus();
        }

        public void TurnOff()
        {
            switchOnOffStatus();
        }

        private void switchOnOffStatus()
        {
            _isSaberOn = !_isSaberOn;
        }

        internal enumSaberColor getColor()
        {
            return _enumSaberColor;
        }
    }
}