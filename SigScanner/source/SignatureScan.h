#pragma once

// Credits to learn_more
// https://github.com/learn-more/findpattern-bench

#define INRANGE(x,a,b)  (x >= a && x <= b) 
#define getBits( x )    (INRANGE((x&(~0x20)),'A','F') ? ((x&(~0x20)) - 'A' + 0xa) : (INRANGE(x,'0','9') ? x - '0' : 0))
#define getByte( x )    (getBits(x[0]) << 4 | getBits(x[1]))
inline DWORD SignatureScan(void* start, DWORD size, const char* szSignature)
{
	const char* pat = szSignature;
	DWORD firstMatch = 0;
	for (auto pCur = (DWORD)start; pCur < (DWORD)start + size; pCur++)
	{
		if (!*pat) return firstMatch;
		if (*(PBYTE)pat == '\?' || *(BYTE*)pCur == getByte(pat))
		{
			if (!firstMatch) firstMatch = pCur;
			if (!pat[2]) return firstMatch;
			if (*(PWORD)pat == '\?\?' || *(PBYTE)pat != '\?') pat += 3;
			else pat += 2;    //one ?
		}
		else
		{
			pat = szSignature;
			firstMatch = 0;
		}
	}
	return NULL;
}