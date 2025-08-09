using System;

namespace projectgodot
{
    public static class EnvironmentHelper
    {
        public static bool IsTestEnvironment()
        {
            return Environment.GetEnvironmentVariable("TEST_ENVIRONMENT") == "true";
        }
    }
}