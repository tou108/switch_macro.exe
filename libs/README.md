# libs/ — Required DLLs

このフォルダに、**NX Macro Controller** のインストールフォルダから以下のDLLをコピーしてください。  
Copy the following DLLs from your **NX Macro Controller** installation directory into this folder:

| DLL ファイル名 | 用途 |
|---|---|
| `ICSharpCode.AvalonEdit.dll` | コードエディタ |
| `BZComponent.dll` | UI コンポーネント |
| `DirectShowLib-2005.dll` | 映像キャプチャ (DirectShow) |
| `Vortice.XInput.dll` | XInput ゲームパッド |
| `SharpDX.DirectInput.dll` | DirectInput ゲームパッド |
| `DiscordRPC.dll` | Discord Rich Presence |
| `RJCP.SerialPortStream.dll` | シリアル通信 |
| `OpenCvSharp.dll` | 画像比較 (OpenCV) |
| `OpenCvSharp.Extensions.dll` | OpenCV 拡張 |
| `Microsoft.CodeAnalysis.dll` | C# スクリプト実行 |
| `Microsoft.CodeAnalysis.Scripting.dll` | スクリプト API |
| `Microsoft.CodeAnalysis.CSharp.Scripting.dll` | C# スクリプト |
| `Microsoft.Scripting.dll` | IronPython |
| `Newtonsoft.Json.dll` | JSON 処理 |
| `NxInterface.dll` | Switch 通信インターフェース |
| `Suites.Utils.dll` | ユーティリティ |
| `PSTaskDialog.dll` | ダイアログ UI |
| `UrlBase64.dll` | Base64 エンコード |

## GitHub Actions でビルドする場合

GitHub Actions は `libs/` フォルダ内のDLLを参照します。  
DLLはライセンスの都合上リポジトリには含めていません。  
ビルド前に `libs/` フォルダへ手動でコピーするか、  
プライベートリポジトリの場合は GitHub Secrets + 事前ダウンロードスクリプトをご利用ください。

## ローカルビルド手順

```powershell
# 1. DLL を libs/ にコピー済みであることを確認
# 2. Visual Studio または MSBuild でビルド
msbuild Switch_macro.csproj /p:Configuration=Release /p:Platform=x64

# または dotnet build (SDK-style プロジェクト)
dotnet build Switch_macro.csproj -c Release
```

出力先: `bin\Release\Switch_macro.exe`
