﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XOutput.Devices;
using XOutput.Devices.Mapper;
using XOutput.Devices.XInput;
using XOutput.Tools;
using XOutput.UI.Windows;

namespace XOutput.UI.Component
{
    public class MappingViewModel : ViewModelBase<MappingModel>
    {
        private GameController controller;

        public MappingViewModel(MappingModel model, GameController controller, XInputTypes inputType) : base(model)
        {
            this.controller = controller;
            var mapperData = controller.Mapper.GetMapping(inputType);
            Model.XInputType = inputType;
            var device = controller.InputDevice;
            Model.Inputs.Add(DisabledInputSource.Instance);
            foreach (var directInput in device.Sources.Where(s => s.Type == InputSourceTypes.Button))
            {
                Model.Inputs.Add(directInput);
            }
            foreach (var directInput in device.Sources.Where(s => s.Type == InputSourceTypes.Axis))
            {
                Model.Inputs.Add(directInput);
            }
            foreach (var directInput in device.Sources.Where(s => s.Type == InputSourceTypes.Slider))
            {
                Model.Inputs.Add(directInput);
            }
            if (mapperData != null && mapperData.InputType == null)
                mapperData.Source = device.Sources.Where(s => s.Type == InputSourceTypes.Button).FirstOrDefault();
            Model.MapperData = mapperData;
            SetSelected(mapperData);
        }

        public void Configure()
        {
            new AutoConfigureWindow(new AutoConfigureViewModel(new AutoConfigureModel(), controller, new XInputTypes[] { Model.XInputType }), false).ShowDialog();
            SetSelected(GetMapperData());
        }

        public void Invert()
        {
            decimal? temp = Model.Max;
            Model.Max = Model.Min;
            Model.Min = temp;
        }

        public void Refresh()
        {
            Model.Refresh();
            SetSelected(GetMapperData());
        }

        protected MapperData GetMapperData()
        {
            return controller.Mapper.GetMapping(Model.XInputType);
        }

        protected void SetSelected(MapperData mapperData)
        {
            if (Helper.DoubleEquals(mapperData.MinValue, Model.XInputType.GetDisableValue()) && Helper.DoubleEquals(mapperData.MaxValue, Model.XInputType.GetDisableValue()))
            {
                Model.SelectedInput = DisabledInputSource.Instance;
                Model.ConfigVisibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                Model.SelectedInput = mapperData.Source;
                Model.ConfigVisibility = System.Windows.Visibility.Visible;
            }
            SelectionChanged(Model.SelectedInput);
        }

        protected void SelectionChanged(InputSource type)
        {
            if (type.Type == InputSourceTypes.Disabled)
            {
                Model.Min = (decimal)(100 * Model.XInputType.GetDisableValue());
                Model.Max = (decimal)(100 * Model.XInputType.GetDisableValue());
                Model.ConfigVisibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                Model.MapperData.Source = type;
                Model.ConfigVisibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
