#include <fmt/core.h>
import std;
import my_module;

// https://learn.microsoft.com/en-us/cpp/c-language/using-wmain
auto wmain([[maybe_unused]] int argc, [[maybe_unused]] wchar_t* argv[], [[maybe_unused]] wchar_t* envp[]) -> int {
    std::println();
    fmt::print("Hello World!\n");

    my_func();

    return 0;
}
