using PodcastManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AllPodcast.Views
{
    public class PodcastListView : Grid
    {
        #region private Variables

        private readonly ObservableCollection<IPodcastEpisode> _itemSource;

        #endregion

        #region Bindable Properties

        public static readonly BindableProperty ItemSourceProperty = BindableProperty.Create(
            propertyName: nameof(ItemSource),
            returnType: typeof(IEnumerable<IPodcastEpisode>),
            declaringType: typeof(PodcastListView),
            defaultValue: default(IEnumerable<IPodcastEpisode>),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnItemSourceChanged);

        #endregion
        
        #region Public Variables

        public IEnumerable<IPodcastEpisode> ItemSource
        {
            get => _itemSource;
            set
            {
                if(value == null)
                    return;

                _itemSource.Clear();
                value.ForEach(i=>_itemSource.Add(i));
            }
        }

        #endregion

        #region Constructors

        public PodcastListView()
        {
            _itemSource = new ObservableCollection<IPodcastEpisode>();
            _itemSource.CollectionChanged += _itemSource_CollectionChanged;
        }

        public PodcastListView(ObservableCollection<IPodcastEpisode> itemSource)
        {
            _itemSource = itemSource;
            _itemSource.CollectionChanged += _itemSource_CollectionChanged;
        }

        #endregion

        #region Private Methods

        private void _itemSource_CollectionChanged(
            object sender, NotifyCollectionChangedEventArgs e)
        {
            var currentAdded = _itemSource.Count;
            var itensCount = ItemSource.ToList().Count;

            if (currentAdded != itensCount)
                return;

            RefreshLayout();
        }
          

        private void RefreshLayout()
        {
            var totalItems = _itemSource.Count;

            Padding = 1;
            ColumnDefinitions.Clear();
            RowDefinitions.Clear();

            ColumnDefinitions.Add(
                new ColumnDefinition(){ Width = new GridLength(1,    GridUnitType.Star) });

            ColumnDefinitions.Add(
                new ColumnDefinition(){ Width = new GridLength(1,    GridUnitType.Star) });

            var totalRows = Convert.ToInt32(
                Math.Floor((totalItems / 2f)), CultureInfo.InvariantCulture);

            for (var i = 0; i < totalRows; i++)
            {
                RowDefinitions.Add(
                    new RowDefinition() {Height = new GridLength(1, GridUnitType.Star)});
            }

            var row = 0;
            var column = 0;

            _itemSource.ForEach(
                i => IncludeItem(i, ref row, ref column));
        }

        private void IncludeItem(IPodcastEpisode i, ref int row, ref int column)
        {
            var viewCell = CreateViewCellFromTemplate(i);
            Children.Add(viewCell, column, row);

            switch (column)
            {
                case 1:
                    column = 0;
                    row++;
                    break;
                default:
                    column++;
                    break;
            }
        }

        private View CreateViewCellFromTemplate(IPodcastEpisode itemEpisode)
        {
            var view = new Grid()
            {
                BackgroundColor = Color.Black,
                RowSpacing = 0,
                ColumnSpacing = 0,
                RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition() {Height = new GridLength(1, GridUnitType.Star)}
                },
                ColumnDefinitions = new ColumnDefinitionCollection()
                {
                    new ColumnDefinition() {Width = new GridLength(1, GridUnitType.Star)}
                }
            };
            
            var coverBackground = new Image()
            {
                Source = ImageSource.FromUri(new Uri(itemEpisode.Image)),
                BackgroundColor = Color.DimGray,
                Aspect = Aspect.AspectFill
            };

            var titleLabel = new Label()
            {
                Text = itemEpisode.Title,
                TextColor = Color.White,
                BackgroundColor = Color.Transparent,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            var imageMask = new Frame
            {
                BackgroundColor = Color.Black,
                CornerRadius = 0,
                Opacity = 0.8,
                HasShadow = true
            };

            var titleGrid = new Grid
            {
                BackgroundColor = Color.Transparent,
                RowSpacing = 0,
                ColumnSpacing = 0,
                RowDefinitions = new RowDefinitionCollection()
                {
                    new RowDefinition() {Height = new GridLength(0.8, GridUnitType.Star)},
                    new RowDefinition() {Height = new GridLength(0.2, GridUnitType.Star)}
                },
                ColumnDefinitions = new ColumnDefinitionCollection()
                {
                    new ColumnDefinition() {Width = new GridLength(1, GridUnitType.Star)}
                }
            };

            var titleMask = new Image
            {
                Aspect = Aspect.Fill,
                Source = ImageSource.FromResource("AllPodcast.Resources.Images.mask.png",
                    typeof(App).GetTypeInfo().Assembly)
            };

            titleGrid.Children.Add(titleMask, 0, 1);
            titleGrid.Children.Add(titleLabel, 0, 1);

            view.Children.Add(imageMask);
            view.Children.Add(coverBackground);
            view.Children.Add(titleGrid);

            return view;
        }

        private static void OnItemSourceChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var newList = (IEnumerable<IPodcastEpisode>) newvalue;
            var control = (PodcastListView)bindable;

            control.ItemSource = newList;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            var colsCount = ColumnDefinitions.Count;
            var rowsCount = RowDefinitions.Count;

            if (colsCount > 0 & rowsCount > 0)
                HeightRequest = (width / colsCount) * rowsCount;
        }

        #endregion
    }
}