using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
namespace H2F
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private bool VerificarCampos()
        {
          
            if (!File.Exists("src/site/index.html"))
            {
                MessageBox.Show("Arquivo index.html faltando na pasta site!");
                return false;
            }            
            return true;
        }
        private string GetSource()
        {

            string Source = Properties.Resources.teste.
                Replace("{WIDTH}", txtWidth.Value.ToString()).
                Replace("{HEIGTH}", txtHeigth.Value.ToString()).
                Replace("{BORDA}", (cbSemBordas.Checked) ? "true" : "false").
                Replace("{TOPMOST}", (cbTopMost.Checked) ? "true" : "false").
                Replace("{SCROLLBAR}", (!cbSemScroll.Checked) ? "true" : "false").
                Replace("{FULLSCREEN}", (cbFullScreen.Checked) ? "true" : "false").
                Replace("{MAXBOX}", ( cbMaximeBox.Checked) ? "true" : "false").
                Replace("{MINBOX}", ( cbMinBox.Checked) ? "true" : "false");

            return Source;
        }
     
        private string LerBytes(byte[] _Bytes)
        {
            string Bytes = "new byte[] {";
            foreach (byte b in _Bytes)
                Bytes += $"0x{b.ToString("X")}, ";
            Bytes = Bytes.Substring(0, Bytes.Length - 2) + "};";
            return Bytes;
        }
        private void tCompilar()
        {
            if (File.Exists("1.zip"))
                File.Delete("1.zip");
            ZipFile.CreateFromDirectory("src/", "1.zip");
            if (VerificarCampos() && File.Exists("1.zip"))
            {                
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Arquivos executavel (*.exe)|*.exe";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string Source = GetSource().Replace("{BYTES}", LerBytes(File.ReadAllBytes("1.zip")));
                    if (icon != null)
                    {

                        if (File.Exists("icon.ico"))
                            File.Delete("icon.ico");
                        File.Copy(icon, "icon.ico");
                        Source = Source.Replace("//{ICONE}", "using(Stream stream =  System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(\"icon.ico\")){ f.Icon = new System.Drawing.Icon(stream); }");
                    }
                    string[] Erros = CodeDOM.Compilar(Source, sfd.FileName, icon).ToArray();
                    if (Erros.Length > 0)
                    {
                        foreach (string s in Erros)
                            MessageBox.Show($"ERROR {s} ", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (MessageBox.Show("Compilado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            Process.Start(sfd.FileName);
                        }
                    }
                }
                if (icon != null)
                    File.Delete("icon.ico");
                File.Delete("1.zip");
            }
        }
        private void btnCompilar_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(tCompilar);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            
        }
        string icon = null;
        private void iTalk_Button_21_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Arquivos de icone (*.ico)|*.ico";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                icon = ofd.FileName;
                pictureBox1.ImageLocation = icon;
            }
        }
    }
}
