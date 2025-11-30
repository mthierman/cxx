using LibGit2Sharp;

namespace CXX;

public static class Vcpkg
{
    public static void Clone()
    {
        var url = "https://github.com/microsoft/vcpkg.git";
        var destination = Path.Combine(App.Paths.AppLocal, "vcpkg");

        var options = new CloneOptions
        {
            Checkout = true,
            RecurseSubmodules = false,
            FetchOptions = { Depth = 1 }
        };

        Repository.Clone(url, destination, options);
    }
}
