# Switch_macro

NX Macro Controller をベースにした PC 版 Switch マクロツール。  
UIタブ配置をリシャッフルしたカスタムビルドです。

## 変更点 (UI ランダム配置)

| 項目 | 変更前 | 変更後 |
|---|---|---|
| メインタブ順序 | コード / ログ / リソース / ショートカット | **ショートカット / ログ / コード / リソース** |
| リソースサブタブ順序 | 画像 / ファイル | **ファイル / 画像** |
| コードタブ ボタン位置 | 上部 (実行・記録・入力補助) | **下部 (実行・記録・入力補助)** |
| ツールバー ボタン順 | マクロ選択 → 接続先 | **接続先 → マクロ選択** |
| ビルド出力ファイル名 | `NX Macro Controller.exe` | **`Switch_macro.exe`** |

## ビルド要件

- Windows 10/11 (x64)
- .NET Framework 4.8
- MSBuild / Visual Studio 2022
- `libs/` フォルダへのDLLコピー（詳細は [libs/README.md](libs/README.md)）

## ビルド方法

```powershell
# DLLをlibsフォルダにコピーしてから:
msbuild Switch_macro.csproj /p:Configuration=Release /p:Platform=x64
# 出力: bin\Release\Switch_macro.exe
```

## GitHub Actions 自動ビルド

`.github/workflows/build.yml` に定義済み。  
`main` / `master` ブランチへのプッシュ時に自動ビルド。  
`v*` タグをプッシュするとGitHub Release が作成されます。

> **注意**: `libs/` 内のDLLはライセンスの関係でリポジトリに含まれていません。  
> GitHub Actions でビルドする場合は、CI ステップでDLLを事前に配置してください。
