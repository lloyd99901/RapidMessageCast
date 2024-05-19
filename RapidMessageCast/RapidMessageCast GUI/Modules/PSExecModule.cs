using System.Diagnostics;

namespace RapidMessageCast_Manager.Modules
{
    internal class PSExecModule
    {
        public static bool isPSExecPresent()
        {
            //Check if PSexec is present and has get the Product Name Sysinternals PsExec
            if (File.Exists("PsExec.exe"))
            {
                FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo("PsExec.exe");
                if (myFileVersionInfo.ProductName == "Sysinternals PsExec")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
