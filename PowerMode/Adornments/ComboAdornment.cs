﻿namespace BigEgg.Tools.PowerMode.Adornments
{
    using System.Collections.Generic;
    using System.Windows.Controls;

    using Microsoft.VisualStudio.Text.Editor;

    using BigEgg.Tools.PowerMode.Services;

    public partial class ComboAdornment
    {
        private const int ADORNMENT_WIDTH = 100;
        private const int ADORNMENT_TITLE_HEIGHT = 15;
        private const int ADORNMENT_COMBO_NUMBER_HEIGHT = 65;
        private const int ADORNMENT_EXCLAMATION_HEIGHT = 30;
        private const double TopMargin = 30;
        private const double RightMargin = 30;
        private Image titleImage;
        private Image comboNumberImage;
        private Image exclamationImage;


        public ComboAdornment()
        {
            this.titleImage = new Image()
            {
                Source = UpdateTitleImage()
            };
            this.comboNumberImage = new Image()
            {
                Source = UpdateComboNumberImage(0).Item1
            };
            this.exclamationImage = new Image()
            {
                Source = UpdateExclamationImage(0)
            };
        }

        public List<Image> OnSizeChanged(IWpfTextView view)
        {
            Canvas.SetLeft(this.titleImage, view.ViewportRight - RightMargin - ADORNMENT_WIDTH);
            Canvas.SetTop(this.titleImage, view.ViewportTop + TopMargin);

            Canvas.SetLeft(this.comboNumberImage, view.ViewportRight - RightMargin - ADORNMENT_WIDTH);
            Canvas.SetTop(this.comboNumberImage, view.ViewportTop + TopMargin + ADORNMENT_TITLE_HEIGHT);

            return new List<Image>()
            {
                titleImage,
                comboNumberImage
            };
        }

        public void OnTextBufferChanged(IAdornmentLayer adornmentLayer, IWpfTextView view, int comboHit)
        {
            adornmentLayer.RemoveAdornment(comboNumberImage);
            var comboNumberImageTuple = UpdateComboNumberImage(comboHit);
            comboNumberImage.Source = comboNumberImageTuple.Item1;
            Canvas.SetLeft(comboNumberImage, view.ViewportRight - RightMargin - ADORNMENT_WIDTH);
            Canvas.SetTop(comboNumberImage, view.ViewportTop + TopMargin + ADORNMENT_TITLE_HEIGHT);
            adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, comboNumberImage, null);

            if (ComboService.ShowExclamation(comboHit))
            {
                adornmentLayer.RemoveAdornment(exclamationImage);
                exclamationImage.Source = UpdateExclamationImage(comboHit);
                Canvas.SetLeft(exclamationImage, view.ViewportRight - RightMargin - ADORNMENT_WIDTH);
                Canvas.SetTop(exclamationImage, view.ViewportTop + TopMargin + ADORNMENT_TITLE_HEIGHT + comboNumberImageTuple.Item2.Height + 5);
                adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, exclamationImage, null);
            }
        }
    }
}