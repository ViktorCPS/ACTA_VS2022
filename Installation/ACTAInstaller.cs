using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Threading;
using System.Reflection;
using IWshRuntimeLibrary;

namespace Installation
{
    [RunInstaller(true)]
    public partial class ACTAInstaller : Installer
    {
        public ACTAInstaller() : base()
        {
            InitializeComponent();
        }

        public override void Commit(System.Collections.IDictionary savedState)
        {
            try
            {
                // install wiaaut.dll nedeed for working with scanner
                base.Commit(savedState);
                string batchPath = System.Environment.SystemDirectory + "\\wia.bat";

                System.Diagnostics.Process.Start(batchPath, System.Environment.SystemDirectory);

                Thread.Sleep(6000);

                System.IO.File.Delete(batchPath);

                // create desktop shortcut
                string workingDIR = Directory.GetParent(Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString()).ToString() + "\\ACTAAdmin";
                string target = workingDIR + "\\ACTAAdmin.exe";
                string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string shortcutFullName = Path.Combine(desktopFolder, "ACTAAdmin" + ".lnk");

                WshShell shell = new WshShellClass();
                IWshShortcut link = (IWshShortcut)shell.CreateShortcut(shortcutFullName);
                link.TargetPath = target;
                link.Description = "ACTAAdmin";
                link.WorkingDirectory = workingDIR;
                link.Save();
            }
            catch { }
        }

        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            try
            {
                base.Uninstall(savedState);

                // delete shortcut
                string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string shortcutFullName = Path.Combine(desktopFolder, "ACTAAdmin" + ".lnk");

                FileInfo shortcut = new FileInfo(shortcutFullName);
                if (shortcut.Exists)
                {
                    shortcut.Delete();
                }
            }
            catch { }
        }
    }
}