/*using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace ReCap.Hub.ViewModels
{
    public partial class SaveGameViewModel : AccountViewModelBase
    {
        const string NAME_EL = "name";
        const string EMAIL_EL = "email";

        string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => RASIC(ref _title, value);
        }

        XDocument _doc = null;
        public void ReadFromXml(string xmlPath)
        {
            string xmlText = File.ReadAllText(xmlPath);
            _doc = XDocument.Parse(xmlText);
            
            Title = _doc.Root.Element(NAME_EL).Value;
        }

        public SaveGameViewModel()
        {

        }


        public static SaveGameViewModel Create(string title)
        {
            SaveGameViewModel saveGame = new SaveGameViewModel()
            {
                Title = title
            };

            return saveGame;
        }
    }
}
*/