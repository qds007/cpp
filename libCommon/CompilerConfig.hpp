#pragma once

#if defined(_MSC_VER)
#pragma warning(disable: 4157 4512)
#endif 

#ifndef _WIN32_WINNT
#define _WIN32_WINNT 0x400
#endif 

#if __cplusplus >=201103L
#define QDS_CPP11 1
#define QDS_OVERRIDE override
#define QDS_FINAL final
#define QDS_EXPLICIT_CONVERSION explicit
#endif

#ifdef WIN32

#define	mb() MemoryBarrier()
#define rmb() MemoryBarrier()

#else
#define mb() asm volatile("mfence":::"memory")
#define rmb() asm volatile("lfence":::"memory")
#endif 



