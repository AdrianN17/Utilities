using System;

namespace RealMachine
{
    internal class RealMachine
    {
        public static void Init()
        {
            int ram;
            int cores;
            SysReq sysreq = new SysReq();

            sysreq.GetSysInfo();
            ram = sysreq.GetRam();
            cores = sysreq.GetRam();
            bool isDebuggerRunning = IsDebuggerPresent();
            bool isSandboxOrVM = (ram < 4000 || cores < 2);

            Console.WriteLine($"RAM : {ram}, Cores: {cores}, Is Debugger Running: {isDebuggerRunning}, Is Sandbox Or VM: {isSandboxOrVM}");
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern bool IsDebuggerPresent();
    }
}
