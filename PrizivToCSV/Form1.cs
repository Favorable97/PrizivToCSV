using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading;
using System.IO;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Diagnostics;
using ClosedXML.Excel;

namespace PrizivToCSV {
    public partial class Form1 : Form {
        [DllImport("user32.dll")]
        static extern int GetWindowThreadProcessId(int hWnd, out int lpdwProcessId);
        public Form1() {
            InitializeComponent();
            openFileDialog1.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
        }

        readonly string connectionString = @"Data Source=SRZ\SRZ;Initial Catalog=Ident;Persist Security Info=True; Pooling=false;Connect Timeout=120; User ID=user;Password=гыук";
        string conString;
        string fileName;
        private void DownloadFileButton_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            fileName = openFileDialog1.FileName;
        }

        private void CreateCSVButton_Click(object sender, EventArgs e) {
            Excel.Application xlAp = new Excel.Application();
            Excel.Workbook xlWb;
            Excel.Worksheet xlSht1;


            xlWb = xlAp.Workbooks.Open(fileName); // открываем файл
            xlSht1 = xlWb.Worksheets[1]; // определяем нужный лист
            string sheet1 = xlSht1.Name.Replace(".", "#") + "$";
            xlSht1 = null;

            GetWindowThreadProcessId(xlAp.Hwnd, out int id);
            Process excelProcess = Process.GetProcessById(id);
            xlAp.Quit();
            excelProcess.Kill();

            conString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = {0}; Extended Properties = 'Excel 8.0;HDR=YES'";

            string formatCon = String.Format(conString, fileName);

            using (OleDbConnection excel_con = new OleDbConnection(formatCon)) {
                excel_con.Open();
                DataTable dtExcel = new DataTable();
                using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con)) {
                    oda.Fill(dtExcel);
                }
                excel_con.Close();

                using (SqlConnection con = new SqlConnection(connectionString)) {
                    using (SqlBulkCopy bulk = new SqlBulkCopy(con)) {
                        bulk.DestinationTableName = "dbo.Prizivi";
                        bulk.ColumnMappings.Add(dtExcel.Columns[0].ColumnName.ToString(), "numb");
                        bulk.ColumnMappings.Add(dtExcel.Columns[1].ColumnName.ToString(), "fam");
                        bulk.ColumnMappings.Add(dtExcel.Columns[2].ColumnName.ToString(), "im");
                        bulk.ColumnMappings.Add(dtExcel.Columns[3].ColumnName.ToString(), "ot");
                        bulk.ColumnMappings.Add(dtExcel.Columns[4].ColumnName.ToString(), "dr");
                        bulk.ColumnMappings.Add(dtExcel.Columns[5].ColumnName.ToString(), "vk");
                        bulk.ColumnMappings.Add(dtExcel.Columns[6].ColumnName.ToString(), "dot");
                        bulk.ColumnMappings.Add(dtExcel.Columns[7].ColumnName.ToString(), "ser");
                        bulk.ColumnMappings.Add(dtExcel.Columns[8].ColumnName.ToString(), "nom");
                        bulk.ColumnMappings.Add(dtExcel.Columns[9].ColumnName.ToString(), "dv");
                        bulk.ColumnMappings.Add(dtExcel.Columns[10].ColumnName.ToString(), "serb");
                        bulk.ColumnMappings.Add(dtExcel.Columns[11].ColumnName.ToString(), "nvb");
                        bulk.ColumnMappings.Add(dtExcel.Columns[12].ColumnName.ToString(), "dvv");
                        bulk.ColumnMappings.Add(dtExcel.Columns[13].ColumnName.ToString(), "prim");

                        con.Open();
                        bulk.WriteToServer(dtExcel);
                        con.Close();
                    }
                }
            }

            string folderName;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.Cancel) {
                return;
            } else {
                folderName = folderBrowserDialog1.SelectedPath;
                using (StreamWriter writer = new StreamWriter(folderName + "\\" + Path.GetFileNameWithoutExtension(fileName.Substring(fileName.LastIndexOf('\\'), fileName.Length - fileName.LastIndexOf('\\'))) + ".csv", false, System.Text.Encoding.Default)) {
                    writer.WriteLine("ФАМИЛИЯ;ИМЯ;ОТЧЕСТВО;ДАТА_РОЖДЕНИЯ;ТИП_УДЛ;СЕРИЯ_УДЛ;НОМЕР_УДЛ;ДАТА_НАЧАЛА;ДАТА_ОКОНЧАНИЯ;СРОК");
                    using (SqlConnection con = new SqlConnection(connectionString)) {
                        con.Open();
                        using (SqlCommand  com = new SqlCommand("Update Prizivi Set ser = '0' + ser where len(ser) < 4", con)) {
                            com.ExecuteNonQuery();
                        }

                        using (SqlCommand com = new SqlCommand("Update Prizivi Set ser = left(ser, 2) + ' ' + right(ser, 2)", con)) {
                            com.ExecuteNonQuery();
                        }

                        using (SqlCommand com = new SqlCommand("Update Prizivi Set nom = '0' + nom where len(nom) < 6", con)) {
                            com.ExecuteNonQuery();
                        }

                        using (SqlCommand com = new SqlCommand("declare @code varchar; declare @srok varchar; set @srok = '1'; set @code = '14'; Select COALESCE(fam, ''), COALESCE(im, ''), COALESCE(ot, ''), COALESCE(dr, ''), @code, COALESCE(ser, ''), COALESCE(nom, ''), COALESCE(dot, ''), DATEADD(year, 1, COALESCE(dot, '01.01.1900')), @srok from Prizivi Order by fam", con)) {
                            using (SqlDataReader reader = com.ExecuteReader()) {
                                reader.Read();

                                while (reader.Read()) {
                                    writer.WriteLine(reader.GetString(0) + ";" + reader.GetString(1) + ";" + reader.GetString(2) + ";" + reader.GetValue(3) + ";" + reader.GetValue(4) + ";" + reader.GetString(5) + ";" + reader.GetString(6) + ";" + reader.GetValue(7) + ";" + reader.GetValue(8) + ";" + reader.GetValue(9));
                                }
                            }
                        }
                    }
                }
            }

            using (SqlConnection con = new SqlConnection(connectionString)) {
                con.Open();
                using (SqlCommand com = new SqlCommand("Delete From Prizivi", con)) {
                    com.ExecuteNonQuery();
                }
            }
            
        }
    }
}
