#pragma once
#include "stdafx.h"

#ifdef _QDS_EXPORTING_libMessage 
	#define DLL_libMessage __declspec(dllexport)
#else
    #define DLL_libMessage __declspec(dllimport)
#endif

#ifdef _QDS_EXPORTING_libService 
#define DLL_libService __declspec(dllexport)
#else
#define DLL_libService __declspec(dllimport)
#endif
