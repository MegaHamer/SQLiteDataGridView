﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations.Model;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLiteDataGridView
{
    public partial class Form1 : Form
    {
        public string filepath = string.Empty;
        SQLiteConnection Connect = new SQLiteConnection();
        SQLiteCommand cmd = new SQLiteCommand();
        SQLiteDataAdapter adapter = new SQLiteDataAdapter();

        public Form1()
        {
            InitializeComponent();
            saveFileDialog1.Filter = "data base files (*.db)|*.db|All files (*.*)|*.*";
            openFileDialog1.Filter = "data base files (*.db)|*.db|All files (*.*)|*.*";
            saveFileDialog1.FileName = "DataBase";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            filepath = saveFileDialog1.FileName;

            if (!File.Exists(filepath))
            {
                //CREATE DB
                SQLiteConnection.CreateFile(filepath);
                //CREATE TABLE
                CreateTable();
                InsertTable();
            }
            openSqlInDataGridView(dataGridView1);
            openSqlInDataGridView(dataGridView2);

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            filepath = openFileDialog1.FileName;
            openSqlInDataGridView(dataGridView1);
            openSqlInDataGridView(dataGridView2);


        }
        public void CreateTable()
        {
            ConnectionOpen();
            string sqlCreateTable =
                "CREATE TABLE IF NOT EXISTS TABLE_NAME_TEST " +
                "(ID INTEGER," +
                "NAME VARCHAR2(20)" +
                ");";
            cmd = new SQLiteCommand(sqlCreateTable, Connect);
            cmd.ExecuteNonQuery();
            ConnectionClose();
        }
        public void InsertTable()
        {
            ConnectionOpen();
            string sqlInsertTable =
                "INSERT INTO TABLE_NAME_TEST (ID,NAME) VALUES(1,'CHORT')";
            cmd = new SQLiteCommand(sqlInsertTable, Connect);
            cmd.ExecuteNonQuery();
            ConnectionClose();
        }
        public void openSqlInDataGridView(DataGridView dgv)
        {
            ConnectionOpen();
            //LINK DATAGRIDVIEW
            string sqlSelectTable = "SELECT * FROM TABLE_NAME_TEST";
            adapter = new SQLiteDataAdapter(sqlSelectTable, Connect);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            dgv.DataSource = dt;

            ConnectionClose();
        }
        public void ConnectionOpen()
        {
            string connectionSting = $"Data Source={filepath}; Version=3;";
            //CONNECT DB
            Connect = new SQLiteConnection(connectionSting);
            Connect.Open();
        }
        public void ConnectionClose()
        {
            Connect.Close();
        }
    }
}