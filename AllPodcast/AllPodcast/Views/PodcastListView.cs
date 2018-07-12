using PodcastManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AllPodcast.Views
{
    public class PodcastListView : Grid
    {
        #region private Variables

        private readonly ObservableCollection<IPodcastEpisode> _itemSource;
        private readonly Assembly _assembly = typeof(App).GetTypeInfo().Assembly;
        private bool _isBusy;
        private bool _canExecute;

        #endregion

        #region Bindable Properties

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            propertyName: nameof(Command),
            returnType: typeof(ICommand),
            declaringType: typeof(PodcastListView),
            defaultValue: default(ICommand),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: CommandChanged);

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

                value.ForEach(i=>
                {
                    if (!_itemSource.Any(s => s.Title.Equals(i.Title) && s.Image.Equals(i.Image)))
                        _itemSource.Add(i);
                });
            }
        }

        public ICommand Command { get; set; }

        #endregion

        #region Constructors

        public PodcastListView(ObservableCollection<IPodcastEpisode> itemSource)
        {
            _itemSource = itemSource ?? new ObservableCollection<IPodcastEpisode>();
            _itemSource.CollectionChanged += _itemSource_CollectionChanged;
            _canExecute = true;
            RefreshLayout();
        }

        public PodcastListView()
        {
            _itemSource = new ObservableCollection<IPodcastEpisode>();
            _itemSource.CollectionChanged += _itemSource_CollectionChanged;
            _canExecute = true;
            RefreshLayout();
        }

        #endregion

        #region Private Methods
        
        private void _itemSource_CollectionChanged(
            object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems == null)
                return;

            var newItems = e.NewItems.Cast<IPodcastEpisode>();
            var currentRow = RowDefinitions.Count - 1;
            var currentcolumn = 1;
            var lastChild = Children.LastOrDefault();

            if (lastChild != null)
            {
                var column = GetColumn(lastChild);

                if (column > 0)
                {
                    currentRow = currentRow + 1 ;
                    currentcolumn =  0;
                }
            }
            else
            {
                currentRow = 0;
                currentcolumn = 0;
            }

             newItems.ForEach(
                i => IncludeItem(i, ref currentRow, ref currentcolumn));
        }
          
        private void RefreshLayout()
        {
            Padding = 1;
            ColumnDefinitions.Clear();
            RowDefinitions.Clear();

            ColumnDefinitions.Add(
                new ColumnDefinition(){ Width = new GridLength(1, GridUnitType.Star) });

            ColumnDefinitions.Add(
                new ColumnDefinition(){ Width = new GridLength(1, GridUnitType.Star) });
            
            var row = 0;
            var column = 0;

            _itemSource.ForEach(
                i => IncludeItem(i, ref row, ref column));
        }

        private void IncludeItem(IPodcastEpisode i, ref int row, ref int column)
        {
            var viewCell = CreateViewCellFromTemplate(i);

            if(RowDefinitions.Count - 1 < row)
                RowDefinitions.Add(
                    new RowDefinition() {Height = new GridLength(1, GridUnitType.Star)});

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

            var tapgesture = new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    if (!_canExecute)
                        return;

                    _canExecute = false;
                    
                    DoAnimation(view);
                    Command?.Execute(itemEpisode);

                    _canExecute = true;
                })
            };
            
            var backgroundLoader = new ActivityIndicator()
            {
                IsVisible = true,
                IsRunning = true,
                IsEnabled = true,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                BackgroundColor = Color.DimGray
            };

            var coverBackground = new Image()
            {
                Source = ImageSource.FromUri(new Uri(itemEpisode.Image)),
                BackgroundColor = Color.Transparent,
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
                Source = ImageSource.FromResource(
                    "AllPodcast.Resources.Images.mask.png",
                    _assembly)
            };

            titleGrid.Children.Add(titleMask, 0, 1);
            titleGrid.Children.Add(titleLabel, 0, 1);

            view.Children.Add(backgroundLoader);
            view.Children.Add(imageMask);
            view.Children.Add(coverBackground);
            view.Children.Add(titleGrid);
            
            view.GestureRecognizers.Add(tapgesture);

            return view;
        }

        private static void OnItemSourceChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var newList = (IEnumerable<IPodcastEpisode>) newvalue;
            var control = (PodcastListView)bindable;

            control.ItemSource = newList;
        }
        
        private static void CommandChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var newCommand = (ICommand) newvalue;
            var control = (PodcastListView)bindable;

            control.Command = newCommand;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            var colsCount = ColumnDefinitions.Count;
            var rowsCount = RowDefinitions.Count;

            if (colsCount > 0 & rowsCount > 0)
                HeightRequest = (width / colsCount) * rowsCount;
        }

        private async void DoAnimation(VisualElement view)
        {
            if(_isBusy)
                return;

            await Animate(view);
        }

        private async Task Animate(VisualElement view)
        {
            _isBusy = true;

            await view.ScaleTo(1 - 0.5, 200, Easing.SpringOut);
            await view.ScaleTo(1, 200, Easing.SpringOut);    
            
            _isBusy = false;
        }

        #endregion
    }
}