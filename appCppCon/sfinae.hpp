#pragma once
#include <type_traits>

int func(...) { return 0; }
int func(float f) { return 1; }

template <typename T>
typename std::enable_if
     <std::is_integral<T>::value, int>::type
func(T t) { return 2; }
