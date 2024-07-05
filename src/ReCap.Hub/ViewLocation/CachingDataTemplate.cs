using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;

namespace ReCap.Hub.Views
{
    public class CachingDataTemplate
        : IRecyclingDataTemplate
        , ITypedDataTemplate
        , ICachingDataTemplate
    {
        readonly DataTemplate _dataTemplate = new();

        [DataType]
        public Type DataType
        {
            get => _dataTemplate.DataType;
            set => _dataTemplate.DataType = value;
        }

        [Content]
        [TemplateContent]
        public object Content
        {
            get => _dataTemplate.Content;
            set => _dataTemplate.Content = value;
        }

        public bool Match(object data)
            => _dataTemplate.Match(data);

        bool _useCaching = true;
        public bool UseCaching
        {
            get => _useCaching;
            set => _useCaching = value;
        }

        public virtual bool SupportsRecycling { get; set;} = false;

        Dictionary<object, Control> _cache = new();
        public Control Build(object data)
            => this.Build_Impl(data, UseCaching, ref _cache, _dataTemplate.Build);

        public Control Build(object data, Control existing)
            => existing ?? this.Build_Impl(data, UseCaching, ref _cache, d => _dataTemplate.Build(d, existing));
    }
}