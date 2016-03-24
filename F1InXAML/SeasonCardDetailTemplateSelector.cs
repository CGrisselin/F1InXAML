using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace F1InXAML
{
    public class SeasonCardDetailTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ActiveTemplate { get; set; }

        public DataTemplate CompletedTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var season = item as Season;
            if (season == null) return null;

            return season.IsInProgress ? ActiveTemplate : CompletedTemplate;
        }
    }
}
