#include "stdafx.h"
#include "SignatureScan.h"

#define ERROR_CHECK(condition, description) \
	if(!(condition)) \
	{ \
		std::wcout << "[ERROR] " << description << ": " << GetLastError() << std::endl; \
		system("pause"); \
		return 1; \
	}

//int main()
int wmain(int argc, wchar_t** argv)
{


	std::wstring tgPath = argv[1];
	std::wstring tgBackupPath = argv[2];

	std::wcout << "Working on: " << tgPath << std::endl;

	const auto file = CreateFile(tgPath.c_str(), GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ, nullptr, OPEN_EXISTING, 0, nullptr);
	if (file == INVALID_HANDLE_VALUE)
	{
		const auto error = GetLastError();
		if (error == ERROR_FILE_NOT_FOUND)
		{
			std::wcout << "[ERROR] Can not find ImagingEngine.dll at path " << tgPath << ".\nDid you place this tool into correct directory?.." << std::endl;
		}
		else if (error == ERROR_SHARING_VIOLATION)
		{
			std::wcout << "[ERROR] File is in use?." << std::endl;
		}
		else
		{
			std::wcout << "[ERROR] " << GetLastError() << std::endl;
		}

		system("pause");
		return 1;
	}

	auto hr = CopyFile(tgPath.c_str(), tgBackupPath.c_str(), false);
	ERROR_CHECK(SUCCEEDED(hr), "Could not create backup");
	std::wcout << "[OK] Backup created." << std::endl;

	const auto size = GetFileSize(file, nullptr);
	ERROR_CHECK(size != INVALID_FILE_SIZE, "GetFileSize");

	const auto fileMapping = CreateFileMappingA(file, nullptr, PAGE_READWRITE, 0, 0, nullptr);
	ERROR_CHECK(fileMapping, "CreateFileMappingA");

	auto map = MapViewOfFile(fileMapping, FILE_MAP_ALL_ACCESS, 0, 0, 0);
	ERROR_CHECK(map, "MapViewOfFile");

	const auto target = SignatureScan(map, size, "85 C0 75 ? ? 0D 03 15 86");
	if (!target)
	{
		std::wcout << "[ERROR] Could not patch dll. Maybe you've already patched it or this tool doesn't support the provided dll." << std::endl;
		system("pause");
		return 1;
	}

	*reinterpret_cast<uint8_t*>(target + 0x2) = 0xEB;

	hr = UnmapViewOfFile(map);
	ERROR_CHECK(SUCCEEDED(hr), "UnmapViewOfFile");

	hr = CloseHandle(fileMapping);
	ERROR_CHECK(SUCCEEDED(hr), "CloseHandle(fileMapping)");

	hr = CloseHandle(file);
	ERROR_CHECK(SUCCEEDED(hr), "CloseHandle(file)");

	std::wcout << "[OK] Patch successfully applied at 0x" << std::hex << std::uppercase << target << " (offset 0x" << std::hex << std::uppercase << (target - reinterpret_cast<DWORD>(map) + 0x2) << ")." << std::endl;
	std::wcout << "In case of errors delete ImagingEngine.dll, rename ImagingEngine.dll.bak to ImagingEngine.dll, then you can try again." << std::endl;
	std::wcout << "\nSupport: https://github.com/PaaaulZ/PhotoViewerOutOfMemoryNoMore" << std::endl;
	system("pause");

	return 0;
}