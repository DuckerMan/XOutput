﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XOutput.Devices
{
    /// <summary>
    /// Event delegate for DeviceDisconnected event.
    /// </summary>
    /// <param name="sender">the disconnected <see cref="IInputDevice"/></param>
    /// <param name="e">event arguments</param>
    public delegate void DeviceDisconnectedHandler(object sender, DeviceDisconnectedEventArgs e);

    /// <summary>
    /// Event argument class for DeviceDisconnected event.
    /// </summary>
    public class DeviceDisconnectedEventArgs : EventArgs
    {

    }
}