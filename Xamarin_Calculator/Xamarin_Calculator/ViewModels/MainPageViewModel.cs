using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin_Calculator.Converters;
using Xamarin_Calculator.Service;

namespace Xamarin_Calculator.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private ICompute getCompute;
        public MainPageViewModel(INavigationService navigationService,ICompute compute)
            : base(navigationService)
        {
            getCompute = compute;
            switch (Device.RuntimePlatform)
            {
                case Device.UWP:
                    Title = "Main Page UWP";
                    break;
                case Device.WPF:
                    Title = "Main Page WPF";
                    break;
                case Device.iOS:
                    Title = "Main Page IOS";
                    break;
                case Device.Android:
                    Title = "Main Page Android";
                    break;
                default:
                    Title = "Main Page";
                    break;
            }

        }
        private DelegateCommand<object[]> btnClickCommand;
        public DelegateCommand<object[]> BtnClickCommand =>
            btnClickCommand ?? (btnClickCommand = new DelegateCommand<object[]>(ExecuteBtnClickCommand));

        void ExecuteBtnClickCommand(object[] parameter)
        {
            var label = (((parameter[1] as StackLayout).Parent as StackLayout).Children[0] as Label);
            if (parameter[0].ToString().Equals("="))
            {
                label.Text = GetCompute.Result(label.Text);
            }
            else if (parameter[0].ToString().Equals("Clear"))
            {
                label.Text = String.Empty;
            }
            else
            {
                label.Text=label.Text+ parameter[0].ToString();
            }

        }

        private DelegateCommand<Element> loaded;
        public DelegateCommand<Element> Loaded =>
            loaded ?? (loaded = new DelegateCommand<Element>((async (p) => { await ExecuteLoadedAsync(p); })));

        public ICompute GetCompute { get => getCompute; set => getCompute = value; }

        async Task ExecuteLoadedAsync(Element element)
        {

            try
            {
                var os = Device.RuntimePlatform;
                await App.Current.MainPage.DisplayAlert("Say", "Hello", "Cancel");
                var stackNumber = ((element as StackLayout).Children[0] as Grid).Children.Where(p => p.StyleId.Equals("StacNumber")).SingleOrDefault() as StackLayout;
                var stackOperation = ((element as StackLayout).Children[0] as Grid).Children.Where(p => p.StyleId.Equals("StacOperation")).SingleOrDefault() as StackLayout;

                if (stackNumber.Children.Count!=0)
                {
                    return;
                }
                
                Task taskNumber = Task.Factory.StartNew(() =>
                {
                    var temp = 2;
                    for (int i = 3; i >= 0; i--)
                    {
                        StackLayout stack = new StackLayout()
                        {
                            StyleId = $"Stac{i}",
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand
                        };

                        for (int j = i * 3; j > 3 * temp; j--)
                        {
                            if (j >= 0)
                            {
                                Button button=null ;                              
                                if (os.Equals(Device.WPF)||os.Equals(Device.UWP))
                                {
                                    button = new Button()
                                    {
                                        Text = $"{j}",
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                        VerticalOptions = LayoutOptions.FillAndExpand,
                                        //WidthRequest = (element as StackLayout).Width / 5
                                    };
                                    button.SetBinding(Button.WidthRequestProperty, "Width", BindingMode.TwoWay,new Divide());
                                    button.BindingContext = element as StackLayout;
                                    
                                }
                                else
                                {
                                    button=new Button() { Text = $"{j}", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                                }
                                button.Command = BtnClickCommand;
                                button.CommandParameter = new object[] { button.Text, element };
                                stack.Children.Add(button);
                            }

                        }
                        temp--;
                        Device.BeginInvokeOnMainThread(() => 
                        {
                            stackNumber.Children.Add(stack);
                        });                        
                    }
                });
                Task taskOperation = Task.Factory.StartNew(() =>
                {
                    for (int i = 3; i >= 0; i--)
                    {
                        StackLayout stack = new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand
                        };                        
                        Button button1 = new Button() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                        Button button2 = new Button() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                        if (os.Equals(Device.WPF) || os.Equals(Device.UWP))
                        {
                            button1.SetBinding(Button.WidthRequestProperty, "Width", BindingMode.TwoWay, new Divide());
                            button2.SetBinding(Button.WidthRequestProperty, "Width", BindingMode.TwoWay, new Divide());
                            
                        }
                        switch (i)
                        {
                            case 0:
                                button1.Text = "=";
                                break;
                            case 1:
                                button1.Text = "*";
                                button2.Text = "/";
                                break;
                            case 2:
                                button1.Text = "+";
                                button2.Text = "-";
                                break;
                            case 3:
                                button1.Text = "Clear";
                                break;
                            default:
                                break;
                        }
                        button1.BindingContext = element as StackLayout;
                        button2.BindingContext = element as StackLayout;
                        button2.Command = BtnClickCommand;
                        button2.CommandParameter = new object[] { button2.Text, element };
                        button1.Command = BtnClickCommand;
                        button1.CommandParameter = new object[] { button1.Text, element };
                        Device.BeginInvokeOnMainThread(() => 
                        {
                            stack.Children.Add(button1);
                            if ( button2.Text!=null)
                            {
                                stack.Children.Add(button2);
                            }                            
                            stackOperation.Children.Add(stack);
                        });
                    }
                });
                await Task.WhenAll(new Task[] { taskNumber, taskOperation });
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

    }
}
