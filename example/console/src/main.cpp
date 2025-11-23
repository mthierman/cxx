#include <fmt/core.h>
import std;
import my_module;

auto wmain() -> int {
    std::println("This app was compiled with cv");
    fmt::println("Testing fmt::print");

    my_func();

    return 0;
}
