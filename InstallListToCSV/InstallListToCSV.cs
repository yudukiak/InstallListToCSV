using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace InstallListToCSV
{
    class InstallListToCSV
    {
        static void Main()
        {
            Console.WriteLine("処理を開始します。");
            var list = new List<List<string>>();
            var dataCurrentUser = GetInstallList("HKEY_CURRENT_USER");
            var dataLocalMachine = GetInstallList("HKEY_LOCAL_MACHINE");
            var listTitle = new List<string>() { "名前", "発行元", "インストール日", "サイズ", "バージョン", "サポートのリンク", "ヘルプのリンク", "更新情報" };
            list.AddRange(dataCurrentUser);
            list.AddRange(dataLocalMachine);
            list.Sort((a, b) => a[0].CompareTo(b[0]));
            list.Insert(0, listTitle);
            Console.WriteLine("処理が完了しました。\n保存を開始します。");
            var fileName = Environment.MachineName + "(" + Environment.UserName + ")";
            using (var writer = new CsvWriter(fileName))
            {
                writer.Write(list);
            }
            Console.WriteLine("保存が完了しました。\n\n続けるにはどれかキーを押してください...");
            Console.ReadKey(true);
        }
        static List<List<string>> GetInstallList(string target)
        {
            var path = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            var list = new List<List<string>>();
            RegistryKey registry = GetRegistryKey(target, path);
            if (registry == null)
            {
                Console.WriteLine("{0}が見つかりませんでした。", target + @"\" + path);
                return list;
            }
            foreach (string key in registry.GetSubKeyNames())
            {
                RegistryKey appKey = GetRegistryKey(target, path + @"\" + key);
                var DisplayName = ValueToString(appKey.GetValue("DisplayName"), key);
                var Publisher = ValueToString(appKey.GetValue("Publisher"), "");
                var InstallDate = ValueToTime(appKey.GetValue("InstallDate"), "");
                var EstimatedSize = ValueToSize(appKey.GetValue("EstimatedSize"), "");
                var DisplayVersion = ValueToString(appKey.GetValue("DisplayVersion"), "");
                //var Contact = ValueToString(appKey.GetValue("Contact"), "");
                var URLInfoAbout = ValueToString(appKey.GetValue("URLInfoAbout"), "");
                var HelpLink = ValueToString(appKey.GetValue("HelpLink"), "");
                var URLUpdateInfo = ValueToString(appKey.GetValue("URLUpdateInfo"), "");
                var appList = new List<string>() { DisplayName, Publisher, InstallDate, EstimatedSize, DisplayVersion, URLInfoAbout, HelpLink, URLUpdateInfo };
                var listNum = list.Count();
                list.Insert(listNum, appList);
            }
            return list;
        }
        static RegistryKey GetRegistryKey(string target, string path)
        {
            if (target == "HKEY_CURRENT_USER") return Registry.CurrentUser.OpenSubKey(path, false);
            return Registry.LocalMachine.OpenSubKey(path, false);

        }
        static string ValueToString(object value, string text)
        {
            if (value == null) return text;
            return value.ToString();
        }
        static string ValueToTime(object value, string text)
        {
            if (value == null) return text;
            var date = value.ToString();
            if (Regex.IsMatch(date, @"^\d{8}$")) return Regex.Replace(date, @"(\d{4})(\d{2})(\d{2})", "$1/$2/$3");
            if (Regex.IsMatch(date, @"^\d{10}$")) return DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(date)).LocalDateTime.ToShortDateString();
            return date;
        }
        static string ValueToSize(object value, string text)
        {
            if (value == null) return text;
            var bytes = Convert.ToDouble(value.ToString());
            var units = new[] {" KB", " MB", " GB", " TB" }; // Bは存在しないっぽい
            var unit = units[0];
            for (var i = 1; i < units.Length; i++)
            {
                if (bytes >= 1024)
                {
                    bytes = bytes / 1024;
                    unit = units[i];
                }
            }
            var integerLength = bytes.ToString("0").Length; // プログラムと機能に表記を合わせたいけど、微妙に計算結果が異なるっぽい
            if (integerLength >= 3) return bytes.ToString("0") + unit;
            if (integerLength >= 2) return bytes.ToString("0.0") + unit;
            return bytes.ToString("0.00") + unit;
        }
    }
}
