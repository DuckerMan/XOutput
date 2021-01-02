﻿namespace XOutput.App.Devices.Input
{
    public class ForceFeedbackTarget
    {
        public string DisplayName => name;
        public IInputDevice InputDevice => inputDevice;
        public int Offset => offset;
        public double Value { get; set; }

        protected IInputDevice inputDevice;
        protected string name;
        protected int offset;

        public ForceFeedbackTarget(IInputDevice inputDevice, string name, int offset)
        {
            this.inputDevice = inputDevice;
            this.name = name;
            this.offset = offset;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
