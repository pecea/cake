// Guids.cs
// MUST match guids.h
using System;

namespace ErnestPrzestrzelskiPiotrSzyperski.Extension
{
    static class GuidList
    {
        public const string guidExtensionPkgString = "8a8b754a-ccd8-461b-899f-2bfedc57f9d8";
        public const string guidExtensionCmdSetString = "88e9dafc-83a7-440e-9e0a-8dbdcc1c2c7b";

        public static readonly Guid guidExtensionCmdSet = new Guid(guidExtensionCmdSetString);
    };
}