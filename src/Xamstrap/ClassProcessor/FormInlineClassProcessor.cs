﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Xamstrap.Enums;

namespace Xamstrap.ClassProcessor
{
    public static partial class Extension
    {
        public static void ProcessInlineClass(this Layout<View> element, double x, double y, double width, double height)
        {
            double xPos = x, yPos = y;
            double lastChildHeight = 0;
            double totalWidth = 0;

            foreach (var child in element.Children)
            {
                var request = child.Measure(width, height);
                var childWidth = request.Request.Width;
                var childHeight = request.Request.Height;
                lastChildHeight = Math.Max(childHeight, lastChildHeight);

                totalWidth += childWidth;
                if (totalWidth > width)
                {
                    yPos += lastChildHeight;
                    xPos = x;
                }

                var region = new Rectangle(xPos, yPos, childWidth, childHeight);
                child.Layout(region);               
                xPos += childWidth;
            }
        }

        public static SizeRequest ProcessFormInlineSizeRequest(this Layout<View> element, double widthConstraint, double heightConstraint)
        {
            if (element.WidthRequest > 0)
                widthConstraint = Math.Min(element.WidthRequest, widthConstraint);
            if (element.HeightRequest > 0)
                heightConstraint = Math.Min(element.HeightRequest, heightConstraint);

            double internalHeight = double.IsPositiveInfinity(heightConstraint) ? double.PositiveInfinity : Math.Max(0, heightConstraint);
            double internalWidth = double.IsPositiveInfinity(widthConstraint) ? double.PositiveInfinity : Math.Max(0, widthConstraint);

            // Measure children height
            double height = 0d;
            foreach (var child in element.Children)
            {
                var size = child.Measure(internalWidth, internalHeight);
                height += size.Request.Height;
            }

            height += element.Padding.VerticalThickness;

            return new SizeRequest(new Size(internalWidth, height), new Size(0, 0));
        }

    }
}
