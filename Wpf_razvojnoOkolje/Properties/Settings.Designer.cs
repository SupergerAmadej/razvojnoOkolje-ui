﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace Wpf_razvojnoOkolje.Properties
{


    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.3.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase
    {

        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public ObservableCollection<ProgJezik> ProgLangs
        {
            get
            {
                return ((ObservableCollection<ProgJezik>)(this["ProgLangs"]));
            }
            set
            {
                this["ProgLangs"] = value;
            }
        }

        //[global::System.Configuration.UserScopedSettingAttribute()]
        //[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        //public ObservableCollection<TipApp> TipApps
        //{
        //    get
        //    {
        //        return ((ObservableCollection<TipApp>)(this["TipApps"]));
        //    }
        //    set
        //    {
        //        this["TipApps"] = value;
        //    }
        //}

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public ObservableCollection<Ogrodje> Ogrodja
        {
            get
            {
                return ((ObservableCollection<Ogrodje>)(this["Ogrodja"]));
            }
            set
            {
                this["Ogrodja"] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Default")]
        public string Theme
        {
            get
            {
                return ((string)(this["Theme"]));
            }
            set
            {
                this["Theme"] = value;
            }
        }
    }
}
