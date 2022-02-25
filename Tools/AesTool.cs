using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace VStoreAPI.Tools
{
    public static class AesTool
    {
        private static readonly string EncryptHash = Startup.StaticConfig["AES_HASH"];
    }
}