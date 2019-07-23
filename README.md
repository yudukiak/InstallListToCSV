# InstallListToCSV

インストール済みのプログラムをCSVとして保存できるソフトです。

-   InstallListToCSV.exeを起動すると`コンピューター名(ユーザー名).csv`が同じディレクトリ内に保存されます。
-   CSVが保存できる以外に機能はありません。
-   アンインストールはInstallListToCSV.exeを削除するだけです。
-   C#を勉強するために作りました。

## CSVの例

```CSV
名前,発行元,インストール日,サイズ,バージョン,サポートのリンク,ヘルプのリンク,更新情報
Audacity 2.3.1,Audacity Team,2019/03/28,60.7 MB,2.3.1,http://audacityteam.org,http://audacityteam.org,http://audacityteam.org
Discord,Discord Inc.,2019/10/20,57.5 MB,0.0.305,,,
DiSpeak,DiSpeak,2019/06/20,67.0 MB,2.4.1,,,
Dropbox,"Dropbox, Inc.",,199 MB,78.3.112,https://www.dropbox.com,https://www.dropbox.com,
```
| 名前           | 発行元          | インストール日 | サイズ  | バージョン | ･･･ |
| -------------- | --------------- | -------------- | ------- | ---------- | --- |
| Audacity 2.3.1 | Audacity Team   | 2019/03/28     | 60.7 MB | 2.3.1      | ･･･ |
| Discord        | Discord Inc.    | 2019/10/20     | 57.5 MB | 0.0.305    | ･･･ |
| DiSpeak        | DiSpeak         | 2019/06/20     | 67.0 MB | 2.4.1      | ･･･ |
| Dropbox        | "Dropbox, Inc." |                | 199 MB  | 78.3.112   | ･･･ |
