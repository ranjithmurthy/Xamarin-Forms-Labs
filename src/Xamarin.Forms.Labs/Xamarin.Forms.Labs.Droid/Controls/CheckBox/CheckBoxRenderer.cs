using System;
using System.ComponentModel;
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Droid.Controls;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Labs.Controls;

[assembly: ExportRenderer(typeof(CheckBox), typeof(CheckBoxRenderer))]

namespace Xamarin.Forms.Labs.Droid.Controls
{
    using NativeCheckBox = Android.Widget.CheckBox;

    public class CheckBoxRenderer : ViewRenderer<CheckBox, Android.Widget.CheckBox>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CheckBox> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged -= ElementOnPropertyChanged;
            }

            if (this.Control == null)
            {
                var checkBox = new NativeCheckBox(this.Context);
                checkBox.CheckedChange += checkBox_CheckedChange;

                this.SetNativeControl(checkBox);
            }

            Control.Text = e.NewElement.Text;
            Control.Checked = e.NewElement.Checked;
            Control.SetTextColor(e.NewElement.TextColor.ToAndroid());

            if (e.NewElement.FontSize > 0)
            {
                Control.TextSize = (float)e.NewElement.FontSize;
            }

            if (!string.IsNullOrEmpty(e.NewElement.FontName))
            {
                Control.Typeface = TrySetFont(e.NewElement.FontName);
            }

            Element.PropertyChanged += ElementOnPropertyChanged;
        }

        //private void CheckedChanged(object sender, EventArgs<bool> eventArgs)
        //{
        //    Device.BeginInvokeOnMainThread(() =>
        //        {
        //            this.Control.Text = this.Element.Text;
        //            this.Control.Checked = eventArgs.Value;
        //        });
        //}

        void checkBox_CheckedChange(object sender, Android.Widget.CompoundButton.CheckedChangeEventArgs e)
        {
            this.Element.Checked = e.IsChecked;
        }

        private void ElementOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "Checked":
                    Control.Text = Element.Text;
                    Control.Checked = Element.Checked;
                    break;
                case "TextColor":
                    Control.SetTextColor(Element.TextColor.ToAndroid());
                    break;
                case "FontName":
                    if (!string.IsNullOrEmpty(Element.FontName))
                    {
                        Control.Typeface = TrySetFont(Element.FontName);
                    }
                    break;
                case "FontSize":
                    if (Element.FontSize > 0)
                    {
                        Control.TextSize = (float)Element.FontSize;
                    }
                    break;
                case "CheckedText":
                case "UncheckedText":
                    Control.Text = Element.Text;
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Property change for {0} has not been implemented.", propertyChangedEventArgs.PropertyName);
                    break;
            }
        }

        private Typeface TrySetFont(string fontName)
        {
            Typeface tf = Typeface.Default;
            try
            {
                tf = Typeface.CreateFromAsset(Context.Assets, fontName);
                return tf;
            }
            catch (Exception ex)
            {
                Console.Write("not found in assets {0}", ex);
                try
                {
                    tf = Typeface.CreateFromFile(fontName);
                    return tf;
                }
                catch (Exception ex1)
                {
                    Console.Write(ex1);
                    return Typeface.Default;
                }
            }
        }
    }
}