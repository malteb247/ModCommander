namespace ModCommander.View
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    public class ViewModelBase : DependencyObject, INotifyPropertyChanged
    {
        public bool IsInDesignTime
        {
            get
            {
                return DesignerProperties.GetIsInDesignMode(this);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void NotifyPropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        public void NotifyPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = GetPropertyName(propertyExpression);
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var body = propertyExpression.Body as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException("Invalid argument", "propertyExpression");
            }

            var property = body.Member as PropertyInfo;

            if (property == null)
            {
                throw new ArgumentException("Argument is not a property", "propertyExpression");
            }

            return property.Name;
        }
    }
}
