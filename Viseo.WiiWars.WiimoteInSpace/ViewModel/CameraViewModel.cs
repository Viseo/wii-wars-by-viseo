using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Viseo.WiiWars.WiimoteInSpace.ViewModel
{
    public class CameraViewModel : NotifierBase
    {
        private const int _stepPxl = 10;
        private Point3D _position;

        public Point3D Position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged();
            }
        }

        private Vector3D _lookDirection;

        public Vector3D LookDirection
        {
            get { return _lookDirection; }
            set
            {
                _lookDirection = value;
                OnPropertyChanged();
            }
        }

        private Vector3D _upDirection;

        public Vector3D UpDirection
        {
            get { return _upDirection; }
            set
            {
                _upDirection = value;
                OnPropertyChanged();
            }
        }


        public enum Movement
        {
            Up,
            Down,
            Left,
            Right,
            Front,
            Back
        }
        public CameraViewModel()
        {
            Position = new Point3D(-200, 0, 0);
            LookDirection = new Vector3D(1, 0, 0);
            UpDirection = new Vector3D(0, 0, -1);
        }

        public void Move(Movement move)
        {
            switch (move)
            {
                case Movement.Up:
                    _position.Z -= _stepPxl;
                    break;
                case Movement.Down:
                    _position.Z += _stepPxl;
                    break;
                case Movement.Left:
                    _position.Y -= _stepPxl;
                    break;
                case Movement.Right:
                    _position.Y += _stepPxl;
                    break;
                case Movement.Front:
                    _position.X += _stepPxl;
                    break;
                case Movement.Back:
                    _position.X -= _stepPxl;
                    break;
                default:
                    break;
            }
            OnPropertyChanged("Position");
        }

        public void LookAt(Point3D point)
        {
            var newLookDirection = new Vector3D(point.X - _position.X,
                                                point.Y - _position.Y,
                                                point.Z - _position.Z);
            newLookDirection.Normalize();

            var diff = _lookDirection - newLookDirection;
            _lookDirection = newLookDirection;
            _upDirection += diff;
            _upDirection.Normalize();

            OnPropertyChanged("LookDirection");
            //OnPropertyChanged("UpDirection");
        }
    }
}
