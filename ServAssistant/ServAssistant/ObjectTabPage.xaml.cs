using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ServAssistant.Modele;

namespace ServAssistant
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ObjectTabPage : TabbedPage
    {
        public ObjectTabPage (Obiekt obiekt)
        {
            InitializeComponent();
            this.Children.Add(new ActionListPage(obiekt) { Title = "Lista prac" });
            this.Children.Add(new ObjectInfo(obiekt) { Title = "Informacje" });
        }
    }
}