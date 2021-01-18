﻿using System;
using System.Windows;

namespace Quan.ControlLibrary
{
    public enum SkinType
    {
        Default
    }

    public class Theme : ResourceDictionary
    {
        public Theme()
        {
            if (DesignerHelper.IsInDesignMode)
            {
                MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri("pack://application:,,,/Quan.ControlLibrary;Component/Themes/ThemeDefault.xaml")
                });
                MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = new Uri("pack://application:,,,/Quan.ControlLibrary;Component/Themes/Generic.xaml")
                });

            }
            else
            {
                UpdateResource();
            }
        }

        private Uri _source;

        public new Uri Source
        {
            get => DesignerHelper.IsInDesignMode ? null : _source;
            set => _source = value;
        }

        private SkinType _skin = SkinType.Default;

        public virtual SkinType Skin
        {
            get => _skin;
            set
            {
                if (_skin == value) return;
                _skin = value;

                UpdateResource();
            }
        }

        public string Name { get; set; }

        public virtual ResourceDictionary GetSkin(SkinType skinType) => ResourceHelper.GetSkin(skinType);

        public virtual ResourceDictionary GetTheme() => new ResourceDictionary
        {
            Source = new Uri("pack://application:,,,/Quan.ControlLibrary;component/Themes/Generic.xaml")
        };

        private void UpdateResource()
        {
            if (DesignerHelper.IsInDesignMode) return;
            MergedDictionaries.Clear();
            MergedDictionaries.Add(GetSkin(Skin));
            MergedDictionaries.Add(GetTheme());
        }
    }
}
