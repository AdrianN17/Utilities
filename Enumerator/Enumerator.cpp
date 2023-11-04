#include <windows.h>
#include <stdlib.h>
#include <tlhelp32.h>
#include <string>

int main() {

    HANDLE hConsole = GetStdHandle(STD_OUTPUT_HANDLE);
    HANDLE hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);

    if (hSnapshot == INVALID_HANDLE_VALUE) {
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED);
        printf("Error creating a process snapshot.\n");
        SetConsoleTextAttribute(hConsole, FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE);
        return 1;
    }

    PROCESSENTRY32 pe32;
    pe32.dwSize = sizeof(PROCESSENTRY32);

    if (Process32First(hSnapshot, &pe32)) {
        do {
            DWORD pid = pe32.th32ProcessID;
            WCHAR name[260];
            wcscpy_s(name, _countof(name), pe32.szExeFile);
            std::string arc = "Unknown";

            // Obtener la arquitectura del proceso
            HANDLE hProcess = OpenProcess(PROCESS_QUERY_INFORMATION, FALSE, pe32.th32ProcessID);
            if (hProcess != NULL) {
                BOOL isWow64 = FALSE;
                IsWow64Process(hProcess, &isWow64);

                if (isWow64) {
                    arc = "32 bits";
                }
                else {
                    arc = "64 bits";
                }

                CloseHandle(hProcess);
            }

            if (arc._Equal("Unknown"))
            {
                SetConsoleTextAttribute(hConsole, FOREGROUND_RED | FOREGROUND_GREEN);
            }
            else if (arc._Equal("32 bits"))
            {
                SetConsoleTextAttribute(hConsole, FOREGROUND_GREEN | FOREGROUND_BLUE);
            }
            else if (arc._Equal("64 bits"))
            {
                SetConsoleTextAttribute(hConsole, FOREGROUND_GREEN);
            }

            printf("PID: %6u\tName: %-50ls\tArchitecture: %s\n", pid, name, arc.c_str());
            SetConsoleTextAttribute(hConsole, FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE);

        } while (Process32Next(hSnapshot, &pe32));
    }

    CloseHandle(hSnapshot);
    return 0;
}
