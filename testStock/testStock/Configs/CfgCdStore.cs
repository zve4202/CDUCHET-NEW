﻿using FirebirdSql.Data.FirebirdClient;
using GH.Configs;
using GH.Database;
using GH.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;
using static GH.Utils.AppHelper;

namespace Tester
{
    public class CfgCdStore : CfgCoreConnection
    {
        [DataMember]
        [DbConnectionProperty(Category = Category.Connection, Caption = "Server", ToolTip = "Сервер для подключения (http://web.firebirdsql.org).\r\nЕсли подключение к серверу на другом компьютере то Remote [V]", SubGroup = "Server")]
        public string DataSource
        {
            get => _dataSource;
            set
            {
                _dataSource = value;
            }
        }
        private string _dataSource = "localhost";

        [DataMember]
        [DbConnectionProperty(Category = Category.Connection, Caption = "Remote",
            ToolTip = "Если подключение к серверу на другом компьютере то Remote [V]", Default = false, EditorType = EditorType.Check, SubGroup = "Server")]
        public bool Remote
        {
            get => _remote;
            set
            {
                _remote = value;
            }
        }
        private bool _remote = false;

        [DataMember]
        [DbConnectionProperty(Category = Category.Connection, Caption = "Port", ToolTip = "Порт для подключения. Обычно 3050",
            Default = 3050, SubGroup = "Database")]
        public int Port { get; set; } = 3050;

        [DataMember]
        [DbConnectionProperty(Category = Category.Connection, Caption = "Charset", ToolTip = "Язык для подключения. Обычно WIN1251", Default = "WIN1251", EditorType = EditorType.Text, SubGroup = "Database")]
        public string Charset { get; set; }

        [DataMember]
        [DbConnectionProperty(Category = Category.Connection, Caption = "Dialect", ToolTip = "Диалект БД. Обычно 3-й", Default = 3, SubGroup = "Database")]
        public int Dialect { get; set; } = 3;

        private List<string> _pathes = new List<string>();
        [DataMember]
        public List<string> Pathes { get => _pathes; set => _pathes = value; }


        private string _database;

        [DataMember]
        [DbConnectionProperty(Category = Category.Connection, Caption = "Path to DB", ToolTip = "Путь к базе данных включая имя файла", EditorType = EditorType.PathSeacher)]
        public string Database
        {
            get => _database;
            set
            {
                if (_database == value)
                    return;
                _database = value;

                if (value != null && !Pathes.Contains(value))
                    Pathes.Add(value);
            }
        }

        [DataMember]
        [DbConnectionProperty(Category = Category.Security, Caption = "User", ToolTip = "Пользователь базы данных")]
        public string UserID { get; set; }

        [DataMember]
        [DbConnectionProperty(Category = Category.Security, Caption = "Password", ToolTip = "Пароль для подключения к базе данных"), PasswordPropertyText]
        public virtual string Password { get; set; }

        [DataMember]
        public FbServerType ServerType { get; private set; }

        protected override void CreateSomething()
        {
            // заглушка
        }

        public override object GetDefault(string name)
        {
            switch (name)
            {
                case nameof(DataSource):
                    return "localhost";
                case nameof(Remote):
                    return false;
                case nameof(Port):
                    return 3050;
                case nameof(Charset):
                    return "WIN1251";
                case nameof(Dialect):
                    return 3;
                case nameof(Database):
                    return Path.Combine(StartupPath, DatabaseFolder) + "CDUCHET.GDB";
                case nameof(UserID):
                    return "SYSDBA";
                case nameof(Password):
                    return "42220";
                case nameof(ServerType):
                    return FbServerType.Default;
                case nameof(UserLogin):
                    return "Виктор";
                case nameof(UserPassword):
                    return "1234567";
                default:
                    return null;
            }
        }
        public override string ConnectionString()
        {
            if (!File.Exists(Database))
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "База GDB|*.GDB|База FDB|*.FDB";
                fileDialog.DefaultExt = Path.GetExtension(Database);
                fileDialog.InitialDirectory = StartupPath;
                if (fileDialog.ShowDialog() == DialogResult.OK && File.Exists(fileDialog.FileName))
                {
                    Database = fileDialog.FileName;
                    Save(true);
                }
            }

            FbConnectionStringBuilder csb = new FbConnectionStringBuilder();

            csb.Pooling = true;
            csb.DataSource = DataSource;
            csb.Dialect = Dialect;// 3;
            csb.Port = Port;// 3050;
            csb.ServerType = ServerType;
            csb.Charset = Charset;// "WIN1251"; //TODO: FbCharset.Windows1251.ToString();
            csb.Database = Database;
            csb.UserID = UserID;
            csb.Password = Password;
            return csb.ConnectionString;
        }



        public override bool TestConnection()
        {
            try
            {
                using (FbConnection conn = new FbConnection(ConnectionString()))
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return false;
            }
        }

        public override bool IsRemoteDataBase()
        {
            return Remote;
        }
    }

}

