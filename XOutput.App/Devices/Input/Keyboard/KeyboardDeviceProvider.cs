﻿using NLog;
using System;
using System.Collections.Generic;
using XOutput.Core.Configuration;
using XOutput.Core.DependencyInjection;

namespace XOutput.App.Devices.Input.Keyboard
{
    public sealed class KeyboardDeviceProvider : IInputDeviceProvider
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        internal const string DeviceId = "keyboard";

        public event DeviceConnectedHandler Connected;
        public event DeviceDisconnectedHandler Disconnected;

        private readonly InputConfigManager inputConfigManager;
        private readonly KeyboardHook hook;
        private bool disposed = false;
        private KeyboardDevice device;

        [ResolverMethod]
        public KeyboardDeviceProvider(InputConfigManager inputConfigManager, KeyboardHook hook)
        {
            this.hook = hook;
            this.inputConfigManager = inputConfigManager;
        }

        public void SearchDevices()
        {
            if (device == null)
            {
                device = new KeyboardDevice(inputConfigManager, hook);
                var config = inputConfigManager.LoadConfig(device);
                device.InputConfiguration = config;
                if (config.Autostart) {
                    device.Start();
                }
                Connected?.Invoke(this, new DeviceConnectedEventArgs(device));
            }
        }

        public IEnumerable<IInputDevice> GetActiveDevices()
        {
            if (device == null)
            {
                return new IInputDevice[] { };
            }
            return new IInputDevice[] { device };
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                if (device != null)
                {
                    device.Dispose();
                }
                hook.Dispose();
            }
            disposed = true;
        }
    }
}
