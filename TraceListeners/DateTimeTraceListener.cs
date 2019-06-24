using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace TraceListeners
{
    //下記をapp.configに追加するとTraceListenerが使えるようになる。
    //app.configはBuild後　projectname.exe.config　になるのでEditorで編集可能
    //保存ファイルパス： 　　　　initializeData=".\Trace\%YYYYMMDD%%SUFFIX%.log" 
    //保存ファイルサイズ： 　　　MaxSize="100000" 
    //%SUFFIX%のフォーマット：　 SuffixFormat="D2"
    //保存エンコード：　         Encoding="utf-8" or "Shift-JIS"
    //%YYYYMMDD%のフォーマット： DateFormat="yyyyMMdd"
    //
    //<system.diagnostics>
    //  <sources>
    //    <source name = "LogSource" switchName="TraceSwitch"
    //      switchType="System.Diagnostics.SourceSwitch" >
    //      <listeners>
    //        <add name = "datetime" />
    //      </ listeners >
    //    </ source >
    //  </ sources >
    //  < switches >
    //    < add name="TraceSwitch" value="Information"/>
    //  </switches>
    //  <sharedListeners>
    //    <add name = "datetime" type="TraceListeners.DateTimeTraceListener, TraceListeners"
    //         initializeData=".\Trace\%YYYYMMDD%%SUFFIX%.log" 
    //         MaxSize="100000" 
    //         SuffixFormat="D2"
    //         Encoding="utf-8"
    //         DateFormat="yyyyMMdd"/>
    //  </sharedListeners>
    //  <trace autoflush = "true" indentsize="4">
    //    <listeners>
    //      <add name = "datetime" />
    //    </ listeners >
    //  </ trace >
    //</ system.diagnostics >

    /// <summary>
    /// DateTimeを足しこんだログを書き込む
    /// ログファイルが最大を超えていたらナンバーをインクリメントする
    /// </summary>
    public class DateTimeTraceListener : TraceListener
    {
        private string _fileNameTemplate = null;
        /// <summary>
        /// ファイル名のテンプレート
        /// </summary>
        private string FileNameTemplate
        {
            get { return _fileNameTemplate; }
        }

        private string _dateFormat = "yyyyMMdd";
        /// <summary>
        /// 日付部分のテンプレート
        /// </summary>
        private string DateFormat
        {
            get { LoadAttribute(); return _dateFormat; }
        }
        private string _suffixFormat = "";
        /// <summary>
        /// ファイルナンバー部分のテンプレート
        /// </summary>
        private string SuffixFormat
        {
            get { LoadAttribute(); return _suffixFormat; }
        }
        private string _datePlaceHolder = "%YYYYMMDD%";
        /// <summary>
        /// ファイル名テンプレートに含まれる日付のプレースホルダ
        /// </summary>
        private string DatePlaceHolder
        {
            get { LoadAttribute(); return _datePlaceHolder; }
        }
        private string _suffixPlaceHolder = "%SUFFIX%";
        /// <summary>
        /// ファイル名テンプレートに含まれるバージョンのプレースフォルダ
        /// </summary>
        private string SuffixPlaceHolder
        {
            get { LoadAttribute(); return _suffixPlaceHolder; }
        }
        private long _maxSize = 10 * 1024 * 1024;
        /// <summary>
        /// トレースファイルの最大バイト数
        /// </summary>
        private long MaxSize
        {
            get { LoadAttribute(); return _maxSize; }
        }
        private Encoding _encoding = Encoding.GetEncoding("utf-8");
        /// <summary>
        /// 出力ファイルのエンコーディング
        /// </summary>
        private Encoding Encoding
        {
            get { LoadAttribute(); return _encoding; }
        }

        #region 内部使用フィールド
        /// <summary>
        /// 出力バッファストリーム
        /// </summary>
        private TextWriter _stream = null;
        /// <summary>
        /// 実際に出力されるストリーム
        /// </summary>
        private Stream _baseStream = null;
        /// <summary>
        /// 現在のログ日付
        /// </summary>
        private DateTime _logDate = DateTime.MinValue;
        /// <summary>
        /// バッファサイズ
        /// </summary>
        private int _bufferSize = 4096;
        /// <summary>
        /// ロックオブジェクト
        /// </summary>
        private object _lockObj = new Object();
        /// <summary>
        /// カスタム属性読み込みフラグ
        /// </summary>
        private bool _attributeLoaded = false;
        #endregion

        /// <summary>
        /// スレッドセーフ
        /// </summary>
        public override bool IsThreadSafe
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileNameTemplate">ファイル名のテンプレート</param>
        public DateTimeTraceListener(string fileNameTemplate)
        {
            _fileNameTemplate = fileNameTemplate;
        }
        /// <summary>
        /// メッセージを出力します
        /// </summary>
        /// <param name="message"></param>
        public override void Write(string message)
        {
            lock (_lockObj)
            {
                if (EnsureTextWriter())
                {
                    if (NeedIndent)
                    {
                        WriteIndent();
                    }
                    _stream.Write(message);
                }
            }
        }
        public override void WriteLine(string message)
        {
            Write(DateTime.Now + " " + message + Environment.NewLine);
        }
        public override void Close()
        {
            lock (_lockObj)
            {
                if (_stream != null)
                {
                    _stream.Close();
                }
                _stream = null;
                _baseStream = null;
            }
        }
        public override void Flush()
        {
            lock (_lockObj)
            {
                if (_stream != null)
                {
                    _stream.Flush();
                }
            }
        }
        /// <summary>
        /// Dispose処理
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Close();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 出力ストリームを準備する
        /// </summary>
        /// <returns></returns>
        private bool EnsureTextWriter()
        {
            if (string.IsNullOrEmpty(FileNameTemplate)) return false;

            DateTime now = DateTime.Now;
            if (_logDate.Date != now.Date)
            {
                Close();
            }
            if (_stream != null && _baseStream.Length > MaxSize)
            {
                Close();
            }
            if (_stream == null)
            {
                string filepath = NextFileName(now);
                // フルパスを求めると同時にファイル名に不正文字がないことの検証
                string fullpath = Path.GetFullPath(filepath);

                StreamWriter writer = new StreamWriter(fullpath, true, Encoding, _bufferSize);
                _stream = writer;
                _baseStream = writer.BaseStream;
                _logDate = now;
            }

            return true;
        }

        /// <summary>
        /// パスで指定されたディレクトリが存在しなければ
        /// 作成します。
        /// </summary>
        /// <param name="dirpath">ディレクトリのパス</param>
        /// <returns>作成した場合はtrue</returns>
        private bool CreateDirectoryIfNotExists(string dirpath)
        {
            if (!Directory.Exists(dirpath))
            {
                // 同時に作成してもエラーにならないため例外処理をしない
                Directory.CreateDirectory(dirpath);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 指定されたファイルがログファイルとして使用できるかの判定を行う
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private bool IsValidLogFile(string filepath)
        {
            if (File.Exists(filepath))
            {
                FileInfo fi = new FileInfo(filepath);
                // 最大サイズより小さければ追記書き込みできるので OK
                if (fi.Length < MaxSize)
                {
                    return true;
                }
                // 最大サイズ以上でもサフィックスをサポートをしていない場合はOK
                if (!FileNameTemplate.Contains(SuffixPlaceHolder))
                {
                    return true;
                }
                // そうでない場合はNG
                return false;
            }
            return true;
        }
        /// <summary>
        /// 日付に基づくサフィックスつきのログファイルのパスを作成する。
        /// </summary>
        /// <param name="logDateTime">ログ日付</param>
        /// <returns></returns>
        private string NextFileName(DateTime logDateTime)
        {
            int suffix = 0;
            string filepath = ResolveFileName(logDateTime, suffix);
            string dir = Path.GetDirectoryName(filepath);
            CreateDirectoryIfNotExists(dir);

            while (!IsValidLogFile(filepath))
            {
                ++suffix;
                filepath = ResolveFileName(logDateTime, suffix);
            }

            return filepath;
        }
        /// <summary>
        /// ファイル名のテンプレートから日付バージョンを置き換えるヘルパ
        /// </summary>
        /// <param name="logDateTime"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private string ResolveFileName(DateTime logDateTime, int suffix)
        {
            string t = FileNameTemplate;
            if (t.Contains(DatePlaceHolder))
            {
                t = t.Replace(DatePlaceHolder, logDateTime.ToString(DateFormat));
            }
            if (t.Contains(SuffixPlaceHolder))
            {
                t = t.Replace(SuffixPlaceHolder, suffix.ToString(SuffixFormat));
            }
            return t;
        }

        #region カスタム属性用
        /// <summary>
        /// サポートされているカスタム属性
        /// MaxSize : ログファイルの最大サイズ
        /// Encoding: 文字コード
        /// DateFormat:ログファイル名の日付部分のフォーマット文字列
        /// VersionFormat: ログファイルのバージョン部分のフォーマット文字列
        /// DatePlaceHolder: ファイル名テンプレートの日付部分のプレースホルダ文字列
        /// VersionPlaceHolder: ファイル名テンプレートのバージョブ部分のプレースホルダ文字列
        /// </summary>
        /// <returns></returns>
        protected override string[] GetSupportedAttributes()
        {
            return new string[] { "MaxSize", "Encoding", "DateFormat", "SuffixFormat"
                , "DatePlaceHolder", "SuffixPlaceHolder" };
        }
        /// <summary>
        /// カスタム属性
        /// </summary>
        private void LoadAttribute()
        {
            if (!_attributeLoaded)
            {
                // 最大バイト数
                if (Attributes.ContainsKey("MaxSize")) { _maxSize = long.Parse(Attributes["MaxSize"]); }
                // エンコーディング
                if (Attributes.ContainsKey("Encoding")) { _encoding = Encoding.GetEncoding(Attributes["Encoding"]); }
                // 日付のフォーマット
                if (Attributes.ContainsKey("DateFormat")) { _dateFormat = Attributes["DateFormat"]; }
                // バージョンのフォーマット
                if (Attributes.ContainsKey("SuffixFormat")) { _suffixFormat = Attributes["SuffixFormat"]; }
                // 日付のプレースホルダ
                if (Attributes.ContainsKey("DatePlaceHolder")) { _datePlaceHolder = Attributes["DatePlaceHolder"]; }
                // ナンバーのプレースホルダ
                if (Attributes.ContainsKey("SuffixPlaceHolder")) { _suffixPlaceHolder = Attributes["SuffixPlaceHolder"]; }

                _attributeLoaded = true;
            }
        }
        #endregion






    }
}
