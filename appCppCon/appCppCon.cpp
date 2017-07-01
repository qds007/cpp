// appCppCon.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "sfinae.hpp"
#include <iostream>

using namespace std;

int main()
{
	cout << func(2.f) << " ";
	cout << func(2.0) << " " ;
	cout << func(2) << " ";
	cout << func(nullptr) << endl;

	int input;
	cin >> input;
    return 0;
}

