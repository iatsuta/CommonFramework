﻿using System.Reflection;

[assembly: AssemblyProduct("CommonFramework")]
[assembly: AssemblyCompany("IvAt")]

[assembly: AssemblyVersion("1.3.0.1")]
[assembly: AssemblyInformationalVersion("changes at build")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif