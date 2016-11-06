using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace JoystickControlDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }


        private void MyJoystick_OnJoystickPressed(object sender, EventArgs e)
        {
            Debug.WriteLine("Pressed");
        }

        private void MyJoystick_OnJoystickMoved(object sender, EventArgs e)
        {
            var joystick = (JoystickUserControl.Joystick)sender;
            Debug.WriteLine("Joystick Moved: " + joystick.XValue + " --- " + joystick.YValue);
        }

        private void MyJoystick_OnJoystickReleased(object sender, EventArgs e)
        {
            Debug.WriteLine("Released");
        }
    }
}
