using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace Xamarin_Calculator.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService) 
            : base (navigationService)
        {
            Title = "Main Page";
        }
       
        private DelegateCommand<Element> loaded;
        public DelegateCommand<Element> Loaded =>
            loaded ?? (loaded = new DelegateCommand<Element>(ExecuteLoaded));

        void ExecuteLoaded(Element element)
        {
            var stackpanel = element as StackLayout;
            var temp = 2;
            for (int i = 3; i >= 0; i--)
            {
                StackLayout stack = new StackLayout()
                {
                    StyleId = $"Stac{i}",
                    Orientation =StackOrientation.Horizontal
                };
                
                for (int j = i*3; j > 3*temp; j--)
                {
                    if (j>=0)
                    {
                        stack.Children.Add(new Button() { Text = $"{j}", HorizontalOptions = LayoutOptions.FillAndExpand,VerticalOptions=LayoutOptions.FillAndExpand });
                    }
                }
                temp--;
                stackpanel.Children.Add(stack);
            }
            
        }

        void c()
        {
            Button button = new Button();
        }
    }
}
