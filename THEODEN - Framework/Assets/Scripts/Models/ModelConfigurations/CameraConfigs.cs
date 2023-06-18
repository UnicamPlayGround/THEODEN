using System;

namespace Models.ModelConfigurations
{
    [Serializable]
    public class CameraConfigs
    {
        public Coordinates position;
        public Coordinates eulerRotation;
        public MinMax angle;
        public MinMax height;
        public MinMax width;
        public MinMax fieldOfView;
        public SpeedModifier speedModifier;
    }
}