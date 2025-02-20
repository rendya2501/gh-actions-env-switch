using System;
using System.IO;

var exportedCount = new Random().Next(0, 2); // 0 または 1 をランダムに出力

// GITHUB_ENV に値を書き込む
using (var writer = new StreamWriter(Environment.GetEnvironmentVariable("GITHUB_ENV"), true))
{
    writer.WriteLine($"EXPORTED_COUNT={exportedCount}");
}

Console.WriteLine($"Exported count: {exportedCount}");
