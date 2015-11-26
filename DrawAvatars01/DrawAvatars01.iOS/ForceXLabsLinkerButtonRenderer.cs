using System;
using DrawAvatars01;
using DrawAvatars01.iOS;
using Xamarin.Forms;
using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ForceXLabsLinkerButton), typeof(ForceXLabsLinkerButtonRenderer))]
namespace DrawAvatars01.iOS
{
    public class ForceXLabsLinkerButtonRenderer : ImageButtonRenderer
    {
        public static void Initialise()
        {
            Console.WriteLine("ForceXLabsLinkerButtonRenderer initialised");
        }
    }
}