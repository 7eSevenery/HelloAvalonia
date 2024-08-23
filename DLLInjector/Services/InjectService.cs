using System;
using System.Runtime.InteropServices;
using System.Text;
using DLLInjector.Services.Interfaces;

namespace DLLInjector.Services;

public class InjectService : IInjectService
{
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType,
        uint flProtect);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize,
        out IntPtr lpNumberOfBytesWritten);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize,
        IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);
    
    const uint PROCESS_ALL_ACCESS = 0x1F0FFF;
    const uint MEM_COMMIT = 0x1000;
    const uint PAGE_READWRITE = 0x04;
    const uint MEM_RELEASE = 0x8000;

    public bool Inject(int pid, string dllPath)
    {
        IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, pid);
        if (hProcess == IntPtr.Zero)
        {
            return false;
        }

        IntPtr allocAddress = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)dllPath.Length, MEM_COMMIT, PAGE_READWRITE);
        if (allocAddress == IntPtr.Zero)
        {
            return false;
        }
        
        byte[] dllPathBytes = Encoding.ASCII.GetBytes(dllPath);
        bool ret = WriteProcessMemory(hProcess, allocAddress, dllPathBytes, (uint)dllPathBytes.Length, out IntPtr bytesWritten);
        if (!ret || bytesWritten.ToInt32() != dllPathBytes.Length)
        {
            return false;
        }

        IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
        if (loadLibraryAddr == IntPtr.Zero)
        {
            return false;
        }

        IntPtr hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, loadLibraryAddr, allocAddress, 0, out IntPtr threadId);
        if (hThread == IntPtr.Zero)
        {
            return false;
        }

        WaitForSingleObject(hThread, 0xFFFFFFFF);
        VirtualFreeEx(hProcess, allocAddress, 0, MEM_RELEASE);
        
        return true;
    }
}