## 目的

C#のコードから出力した値をGitHub Actionsで使用出来るかチェックしたい。  

## C#コード

0か1の数値を `EXPORTED_COUNT` 変数に代入するだけのコード。  

``` cs
using System;
using System.IO;

var exportedCount = new Random().Next(0, 2); // 0 または 1 をランダムに出力

// NULLチェック
var githubEnvPath = Environment.GetEnvironmentVariable("GITHUB_ENV");
if (githubEnvPath == null)
{
    throw new Exception("Environment.GetEnvironmentVariable(GITHUB_ENV) is null !!");
}

// GITHUB_ENV に値を書き込む
using (var writer = new StreamWriter(githubEnvPath, true))
{
    writer.WriteLine($"EXPORTED_COUNT={exportedCount}");
}
```

## Actions YAML

出力された`EXPORTED_COUNT`変数を確認し、条件分岐などを組み込んだYAML。  
当初の目的である、出力値を使用できる事を確認した。  

``` yml
name: GitHub Actions Env Test

on:
  workflow_dispatch:

jobs:
  test_env:
    runs-on: ubuntu-latest

    steps:
      - name: Checkout repository
        uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0'

      - name: Run script
        run: dotnet run --project ./src/sample.csproj

      # 変数がどのように出力されるのかテスト
      - name: Check exported count
        run: | 
          echo "EXPORTED_COUNT is $EXPORTED_COUNT"
          echo "EXPORTED_COUNT is {{ $EXPORTED_COUNT }}"
          echo "EXPORTED_COUNT is" $EXPORTED_COUNT
          echo "EXPORTED_COUNT is" {{ $EXPORTED_COUNT }}
        # invalid pattern
        # echo "EXPORTED_COUNT is ${{ EXPORTED_COUNT }}"
        # echo "EXPORTED_COUNT is" ${{ EXPORTED_COUNT }}

      # $EXPORTED_COUNTが０の場合に実行される処理
      - name: Echo when exported_count is 0
        if: env.EXPORTED_COUNT == '0'
        run: echo "HOGEHOGEHOGEHOGEHOGE"

      # $EXPORTED_COUNTが０ではない場合に実行される処理
      - name: Echo when exported_count is not 0
        if: env.EXPORTED_COUNT != '0'
        run: echo "FUGAFUGAFUGAFUGAFUGA"

      # bashのifで数値チェックによる分岐のテスト
      - name: Bash if Number Check
        run: |
          if [ $EXPORTED_COUNT == 0 ]; then
            echo "000000000000000000000000"
          else
            echo "111111111111111111111111"
          fi

      # bashのifで文字列チェックによる分岐のテスト
      - name: Bash if String Check
        run: |
          if [ "$EXPORTED_COUNT" == "0" ]; then
            echo "000000000000000000000000"
          else
            echo "111111111111111111111111"
          fi
      
      # 変数を変更できるかチェック
      # ValidChangeのスコープだけなら有効であることを確認した
      - name: ValidChange
        run: | 
          echo "Changed"
          echo "EXPORTED_COUNT is $EXPORTED_COUNT"
          echo "EXPORTED_COUNT is {{ $EXPORTED_COUNT }}"
          echo "EXPORTED_COUNT is" $EXPORTED_COUNT
          echo "EXPORTED_COUNT is" {{ $EXPORTED_COUNT }}
        env:
          EXPORTED_COUNT: 3

      # ValidChangeでの変更が反映されているかチェック
      # →変更が反映されないことを確認した。変更範囲はローカルのみである模様。
      - name: Check exported count
        run: | 
          echo "EXPORTED_COUNT is $EXPORTED_COUNT"
          echo "EXPORTED_COUNT is {{ $EXPORTED_COUNT }}"
          echo "EXPORTED_COUNT is" $EXPORTED_COUNT
          echo "EXPORTED_COUNT is" {{ $EXPORTED_COUNT }}
```

## 結果

![image](https://github.com/user-attachments/assets/ffe2ff72-fe08-4257-8102-35ffc3fbfee0)

