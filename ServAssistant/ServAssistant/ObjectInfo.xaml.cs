using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ServAssistant.Modele;
using Newtonsoft.Json;

namespace ServAssistant
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ObjectInfo : ContentPage
	{
        public Dictionary<string, int> info;
		public ObjectInfo (Obiekt obiekt)
		{
			InitializeComponent();
            info = JsonConvert.DeserializeObject<Dictionary<string, int>>(obiekt.info);
            foreach (var item in info)
                additionalInfo.Text += item.Key + " -> " + item.Value.ToString() + "\n";
		}
	}
}