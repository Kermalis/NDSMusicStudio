﻿using System;
using System.Windows.Forms;

namespace Kermalis.NDSMusicStudio
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(UI.MainForm.Instance);

            // Bad coding that I have to include the following line, but I legitimately don't know why a system thread was remaining alive
            Environment.Exit(0);
            // TODO: Check if SoundMixer.@out.Stop() fixes it
        }
    }
}
