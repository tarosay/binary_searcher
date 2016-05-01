using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2つのバイナリファイルを比較する
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.AppendText("　ファイルをドラッグしてね。\r\n");            
            textBox1.AppendText("　一番大きなサイズのファイルの中身を検索して、他のファイルと同じデータが格納されている場所を検索します。\r\n");
        }

        private void Hikaku(string[] files)
        {
            if (files.Length < 2)
            {
                textBox1.AppendText("2つ以上のファイルをドラッグしてください\r\n");
            }

            textBox1.AppendText("開始します\r\n");
            Application.DoEvents();

            try
            {
                //ファイルサイズ取得
                string bigFile = files[0];
                List<FileInfo> searchFiles = new List<FileInfo>();

                FileInfo maxFile = new FileInfo(files[0]);
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo file = new FileInfo(files[i]);
                    searchFiles.Add(file);

                    if (maxFile.Length < file.Length)
                    {
                        maxFile = file;
                    }
                }

                textBox1.AppendText(Path.GetFileName(maxFile.Name) + " からデータを検索します。\r\n");



                byte[] maxdat = null;
                byte[] dat = null;

                //バイナリを読み込みます
                if (File.Exists(bigFile))
                {
                    maxdat = File.ReadAllBytes(maxFile.Directory + "\\" + maxFile.Name);
                }


                for (int k = 0; k < searchFiles.Count; k++)
                {
                    if (maxFile.Length == searchFiles[k].Length)
                    {
                        continue;
                    }

                    if (File.Exists(searchFiles[k].Directory + "\\" + searchFiles[k].Name))
                    {
                        dat = File.ReadAllBytes(searchFiles[k].Directory + "\\" + searchFiles[k].Name);
                    }
                    else
                    {
                        continue;
                    }



                    int j = 0;
                    for (int i = 0; i < maxdat.Length; i++)
                    {
                        if (maxdat[i] == dat[j])
                        {
                            j++;
                            if (j >= dat.Length)
                            {
                                int add = i - j + 1;
                                textBox1.AppendText("　　" + searchFiles[k].Name + " が、アドレス: " + add.ToString("X") + "～ に見つかりました。\r\n");
                                j = 0;
                            }
                        }
                        else
                        {
                            j = 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }

            textBox1.AppendText("検索は終了しました。\r\n");
        }


        #region //          ドラッグドロップ        //
        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            // ドラッグ＆ドロップされたファイル
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            Hikaku(files);
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            // ドラッグ＆ドロップされたファイル
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            Hikaku(files);
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }
        #endregion
    }
}
