using System;
using System.IO;
using System.Windows.Forms;

namespace CityAppBuilder
{
    class Util
    {
        String workingDir = Path.GetTempPath() + "City App Builder\\";

        public void LoadAssets()
        {
            //Copy Assets
            if (!Directory.Exists(workingDir))
            {
                Directory.CreateDirectory(workingDir);
                File.WriteAllBytes(workingDir + "apksigner.jar", Properties.Resources.apksigner);
                File.WriteAllBytes(workingDir + "apktool.jar", Properties.Resources.apktool);
                File.WriteAllBytes(workingDir + "CityApp.apk", Properties.Resources.CityApp);
                File.WriteAllBytes(workingDir + "debug.keystore", Properties.Resources.debug);
            }

            File.Delete(workingDir + "EditedApp.apk");
            File.Delete(workingDir + "log.txt");
        }

        public Boolean BuildApp()
        {
            Boolean buildSuccess = false;

            //Build App
            RunJar("-jar apktool.jar b CityApp -o EditedApp.apk");

            //Sign App
            RunJar("-jar apksigner.jar sign --ks debug.keystore --ks-pass pass:android EditedApp.apk");

            //Show Save prompt
            SaveFileDialog saveAppDialog = new SaveFileDialog();
            saveAppDialog.FileName = "Your App";
            saveAppDialog.Filter = "Android Application Package|*.apk";
            if(File.Exists(workingDir + "EditedApp.apk"))
            {
                buildSuccess = true;
                if (saveAppDialog.ShowDialog() == DialogResult.OK)
                {
                    String newDirectory = saveAppDialog.FileName;
                    File.Copy(workingDir + "EditedApp.apk", newDirectory, true);
                }
            }

            return buildSuccess;            
        }

        public void RunJar(String args)
        {
            //Create process
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();

            //strCommand is path and file name of command to run
            pProcess.StartInfo.FileName = "java.exe";

            //strCommandParameters are parameters to pass to program
            pProcess.StartInfo.Arguments = args;

            pProcess.StartInfo.UseShellExecute = false;

            //Set output of program to be written to process output stream
            pProcess.StartInfo.RedirectStandardOutput = true;

            //Optional
            pProcess.StartInfo.WorkingDirectory = workingDir;

            //Start the process
            pProcess.Start();

            //Get program output
            string strOutput = pProcess.StandardOutput.ReadToEnd();
            File.AppendAllText(workingDir + "log.txt", strOutput);

            //Wait for process to finish
            pProcess.WaitForExit();
        }

        public void WriteResource(String filename, String resource)
        {
            //Decompile App
            if (!Directory.Exists(workingDir + "CityApp"))
            {
                RunJar("-jar apktool.jar d CityApp.apk");
            }

            String valuesDir = workingDir + "CityApp/res/values/";
            File.WriteAllText(valuesDir + filename, resource);
        }

    }
}
