using System;
using System.Runtime.InteropServices;

namespace RealMachine
{
    internal class SysReq
    {
        private int ram;
        private int cores;

        public SysReq()
        {
            this.ram = 0;
            this.cores = 0;
        }

        public int GetRam()
        {
            return ram;
        }

        public int GetCores()
        {
            return cores;
        }

        public int RamDetect()
        {
            MEMORYSTATUSEX memInfo = new MEMORYSTATUSEX();
            memInfo.dwLength = (uint)Marshal.SizeOf(memInfo);
            GlobalMemoryStatusEx(ref memInfo);
            ram = (int)(memInfo.ullTotalPhys / 1024 / 1024);
            return ram;
        }

        public int CoresDetect()
        {
            SYSTEM_INFO sysInfo = new SYSTEM_INFO();
            GetSystemInfo(ref sysInfo);
            cores = (int)sysInfo.dwNumberOfProcessors;
            return cores;
        }

        public void GetSysInfo()
        {
            this.ram = RamDetect();
            this.cores = CoresDetect();
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO
        {
            public ushort wProcessorArchitecture;
            public ushort wReserved;
            public uint dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public IntPtr dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public ushort wProcessorLevel;
            public ushort wProcessorRevision;
        }

        [DllImport("kernel32.dll")]
        public static extern void GetSystemInfo(ref SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll")]
        public static extern void GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);
    }

}
