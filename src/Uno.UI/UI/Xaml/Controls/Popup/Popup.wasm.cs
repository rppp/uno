﻿using Uno.Extensions;
using Uno.Disposables;
using Uno.Logging;
using Windows.UI.Xaml.Controls.Primitives;
using System;
using Windows.UI.Xaml.Media;

namespace Windows.UI.Xaml.Controls
{
	public partial class Popup
	{
		private readonly SerialDisposable _closePopup = new SerialDisposable();

		partial void InitializePartial()
		{
			PopupPanel = new PopupPanel(this);
		}

		protected override void OnChildChanged(FrameworkElement oldChild, FrameworkElement newChild)
		{
			base.OnChildChanged(oldChild, newChild);

			PopupPanel.Children.Remove(oldChild);
			PopupPanel.Children.Add(newChild);
		}

		protected override void OnIsLightDismissEnabledChanged(bool oldIsLightDismissEnabled, bool newIsLightDismissEnabled)
		{
			base.OnIsLightDismissEnabledChanged(oldIsLightDismissEnabled, newIsLightDismissEnabled);

			if (PopupPanel != null)
			{
				PopupPanel.Background = GetPanelBackground();
			}
		}

		protected override void OnIsOpenChanged(bool oldIsOpen, bool newIsOpen)
		{
			base.OnIsOpenChanged(oldIsOpen, newIsOpen);

			if (this.Log().IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug))
			{
				this.Log().Debug($"Popup.IsOpenChanged({oldIsOpen}, {newIsOpen})");
			}

			if (newIsOpen)
			{
				_closePopup.Disposable = Window.Current.OpenPopup(this);
				PopupPanel.Visibility = Visibility.Visible;
			}
			else
			{
				_closePopup.Disposable = null;
				PopupPanel.Visibility = Visibility.Collapsed;
			}
		}

		partial void OnPopupPanelChangedPartial(PopupPanel previousPanel, PopupPanel newPanel)
		{
			previousPanel?.Children.Clear();

			if (newPanel != null)
			{
				if (Child != null)
				{
					newPanel.Children.Add(Child);
				}
				newPanel.Background = GetPanelBackground();
			}
		}

	}
}
