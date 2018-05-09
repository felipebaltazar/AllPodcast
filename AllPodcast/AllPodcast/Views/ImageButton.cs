using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AllPodcast.Views
{
    public sealed class ImageButton : Image
    {
        #region Bindable Properties

        public static readonly BindableProperty ButtonPressedProperty = BindableProperty.Create(
            propertyName: nameof(ButtonPressed),
            returnType: typeof(Command),
            declaringType: typeof(ImageButton),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnButtonPressedUpdated);

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            propertyName: nameof(CommandParameter),
            returnType: typeof(object),
            declaringType: typeof(ImageButton),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnCommandParameterUpdated);


        #endregion

        #region Bind Events

        private static void OnButtonPressedUpdated(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            var control = (ImageButton)bindable;
            var newCommand = (ICommand)newValue;
            
            control.ButtonPressed = newCommand ;
        }

        private static void OnCommandParameterUpdated(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            var control = (ImageButton)bindable;
            control.CommandParameter = newValue;
        }


        #endregion

        #region Local Variables
        
        private bool _isSelected;
        private bool _canExecute = true;
        private const int DefaultScale = 1;
        private object _commandParameter;
        private ICommand _onButtonPressed;

        #endregion

        #region Accessible Variables
        
        public object CommandParameter
        {
            get => _commandParameter ?? (_commandParameter = this);
            set
            {
                if (value == _commandParameter)
                    return;

                _commandParameter = value;
                OnPropertyChanged(nameof(CommandParameter));
            }
        }

        public ICommand ButtonPressed
        {
            get => _onButtonPressed;
            set
            {
                if (_onButtonPressed == value || value == null)
                    return;

                _onButtonPressed = value;
                OnPropertyChanged(nameof(ButtonPressed));
            }
        }
        
        public bool CanExecute
        {
            get => _canExecute;
            private set
            {
                if (value == _canExecute) 
                    return;

                _canExecute = value;
                CanExecuteFunc();
                OnPropertyChanged(nameof(CanExecute));
            }
        }
        
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value)
                    return;

                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        
        #endregion

        #region Constructors

        public ImageButton()
        {
            var onTouchedCommand = new Command(
                async () => await OnTouchedCommandExecute(), CanExecuteFunc);

            var tapGestureRecognizer = new TapGestureRecognizer
            {
                Command = onTouchedCommand
            };

            GestureRecognizers.Add(tapGestureRecognizer);
        }
        
        #endregion

        #region Actions

        private async Task OnTouchedCommandExecute()
        {
            CanExecute = false;
            await Animate();
            _onButtonPressed?.Execute(CommandParameter);
            CanExecute = true;
        }

        private async Task Animate()
        {
            await this.ScaleTo(DefaultScale - 0.5, 250, Easing.SpringOut);
            await this.ScaleTo(DefaultScale, 250, Easing.SpringOut);            
        }

        private bool CanExecuteFunc() => 
            CanExecute && Math.Abs(Scale - 1) < 0;

        #endregion
    }
}
