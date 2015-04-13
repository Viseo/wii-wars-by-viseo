using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiimoteLib;

namespace Viseo.WiiWars.WiimoteInSpace.ViewModel
{
    public class WiimoteButtonsEvents
    {
        private ButtonState _lastButtonState;

        public enum Button
        {
            Up,
            Down,
            Left,
            Right,
            A,
            B,
            Minus,
            Plus,
            Home,
            One,
            Two
        }
        public event ButtonPressedEventHandler ButtonPressed;
        public delegate void ButtonPressedEventHandler(object sender, ButtonPressedEventArgs e);

        public WiimoteButtonsEvents()
        {
            _lastButtonState.A =
                _lastButtonState.B =
                _lastButtonState.Down =
                _lastButtonState.Home =
                _lastButtonState.Left =
                _lastButtonState.Minus =
                _lastButtonState.One =
                _lastButtonState.Plus =
                _lastButtonState.Right =
                _lastButtonState.Two =
                _lastButtonState.Up = false;
        }

        protected virtual void OnButtonPressed(ButtonPressedEventArgs e)
        {
            if (ButtonPressed != null)
                ButtonPressed(this, e);
        }

        public void WiimoteChanged(object sender, WiimoteChangedEventArgs e)
        {
            var currentButtonState = e.WiimoteState.ButtonState;
            if (_lastButtonState.A == false && currentButtonState.A == true)
                OnButtonPressed(new ButtonPressedEventArgs(Button.A));
            if (_lastButtonState.B == false && currentButtonState.B == true)
                OnButtonPressed(new ButtonPressedEventArgs(Button.B));
            if (_lastButtonState.Down == false && currentButtonState.Down == true)
                OnButtonPressed(new ButtonPressedEventArgs(Button.Down));
            if (_lastButtonState.Home == false && currentButtonState.Home == true)
                OnButtonPressed(new ButtonPressedEventArgs(Button.Home));
            if (_lastButtonState.Left == false && currentButtonState.Left == true)
                OnButtonPressed(new ButtonPressedEventArgs(Button.Left));
            if (_lastButtonState.Minus == false && currentButtonState.Minus == true)
                OnButtonPressed(new ButtonPressedEventArgs(Button.Minus));
            if (_lastButtonState.One == false && currentButtonState.One == true)
                OnButtonPressed(new ButtonPressedEventArgs(Button.One));
            if (_lastButtonState.Plus == false && currentButtonState.Plus == true)
                OnButtonPressed(new ButtonPressedEventArgs(Button.Plus));
            if (_lastButtonState.Right == false && currentButtonState.Right == true)
                OnButtonPressed(new ButtonPressedEventArgs(Button.Right));
            if (_lastButtonState.Two == false && currentButtonState.Two == true)
                OnButtonPressed(new ButtonPressedEventArgs(Button.Two));
            if (_lastButtonState.Up == false && currentButtonState.Up == true)
                OnButtonPressed(new ButtonPressedEventArgs(Button.Up));

            _lastButtonState.A = currentButtonState.A;
            _lastButtonState.B = currentButtonState.B;
            _lastButtonState.Down = currentButtonState.Down;
            _lastButtonState.Home = currentButtonState.Home;
            _lastButtonState.Left = currentButtonState.Left;
            _lastButtonState.Minus = currentButtonState.Minus;
            _lastButtonState.One = currentButtonState.One;
            _lastButtonState.Plus = currentButtonState.Plus;
            _lastButtonState.Right = currentButtonState.Right;
            _lastButtonState.Two = currentButtonState.Two;
            _lastButtonState.Up = currentButtonState.Up;
        }

        public class ButtonPressedEventArgs : EventArgs
        {
            public Button Button { get; private set; }

            public ButtonPressedEventArgs(Button button)
            {
                Button = button;
            }
        }
    }
}
