﻿using GH.Database;
using GH.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.ComponentModel;
using System.IO;

namespace GH.Configs
{
    public class CfgCore : AbstractEntity
    {

        public CfgCore()
        {

            IniHelper.IniFile.TryGetValue(this.GetName(), out CfgCore cfg);
            if (cfg == null)
            {
                IniHelper.IniFile.AddInstance(this);
                Load();
            }
            else
                Assigne(cfg);
        }

        #region поля и свойства

        internal static bool _loading = false;

        #endregion

        internal string ConfigPath
        {
            get
            {


                if (this is CfgApp cfgApp)
                    return Path.ChangeExtension(AppHelper.ExePath, ".ini");
                return Path.Combine(AppHelper.StartupPath, AppHelper.ConfigsFolder, GetName()) + ".ini";
            }
        }


        public virtual string GetName()
        {
            return GetType().Name;
        }


        #region методы


        public void Load()
        {
            if (_loading)
                return;
            _loading = true;
            try
            {
                LoadDefaults();
                CfgCore cfg = null;

                FileInfo _fileInfo = new FileInfo(ConfigPath);
                if (!_fileInfo.Exists)
                {
                    Save(true);
                    return;
                }

                Type type = GetType();

                string json = File.ReadAllText(_fileInfo.FullName, System.Text.Encoding.UTF8);
                if (string.IsNullOrEmpty(json))
                {
                    Save(true);
                    return;
                }

                JsonSerializerSettings ser = new JsonSerializerSettings();

                ser.ContractResolver = new CamelCasePropertyNamesContractResolver();
                try
                {
                    cfg = JsonConvert.DeserializeObject(json, type, ser) as CfgCore;
                    Assigne(cfg);
                }
                catch (Exception ex)
                {

                    Logger.Fatal(ex);
                    Save(true);
                }
            }
            finally
            {
                _loading = false;
            }
        }

        protected virtual void LoadDefaults()
        {
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(this, false))
            {
                if (property.Attributes[typeof(UpdatablePropertyAttribute)] is UpdatablePropertyAttribute att)
                    Default(property, att.Default);
            }

        }

        protected void Default(PropertyDescriptor property, object val)
        {
            if (property.GetValue(this) != null)
                return;

            if (val != null)
                property.SetValue(this, val);
            else
                property.SetValue(this, GetDefault(property.Name));
        }

        public virtual object GetDefault(string name)
        {
            throw new NotImplementedException(nameof(GetDefault));
        }


        protected virtual void CreateSomething()
        {
            throw new NotImplementedException(nameof(CreateSomething));
        }

        public void Save(bool anything = false)
        {
            if (!(anything || HasChanges))
                return;
            FileInfo _fileInfo = new FileInfo(ConfigPath);
            Type type = GetType();

            Directory.CreateDirectory(Path.GetDirectoryName(_fileInfo.FullName));
            if (_fileInfo.Exists)
                File.Delete(_fileInfo.FullName);

            JsonSerializerSettings ser = new JsonSerializerSettings();
            ser.ContractResolver = new CamelCasePropertyNamesContractResolver();

            try
            {
                string json = JsonConvert.SerializeObject(this, ser);
                using (StreamWriter sw = new StreamWriter(_fileInfo.FullName, false, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine(json);
                }

            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
            }

            EndEdit();
        }
        #endregion
    }
}
