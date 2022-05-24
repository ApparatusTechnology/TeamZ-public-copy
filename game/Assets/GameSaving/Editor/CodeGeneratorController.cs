using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CodeGeneratorController
{
    [MenuItem("Assets/Start Code Generation")]
    private static void StartCodeGeneration()
    {
        Process.Start(@"Assets\GameSaving\Nugets\TypePack.Cli.exe", $@" ""Game.csproj"" --force");
    }
}
