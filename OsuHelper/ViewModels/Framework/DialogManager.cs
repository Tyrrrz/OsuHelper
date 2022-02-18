using System;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using Stylet;

namespace OsuHelper.ViewModels.Framework
{
    public class DialogManager
    {
        private readonly IViewManager _viewManager;

        public DialogManager(IViewManager viewManager)
        {
            _viewManager = viewManager;
        }

        public async Task<T?> ShowDialogAsync<T>(DialogScreen<T> dialogScreen)
        {
            var view = _viewManager.CreateAndBindViewForModelIfNecessary(dialogScreen);

            void OnDialogOpened(object? sender, DialogOpenedEventArgs openArgs)
            {
                void OnScreenClosed(object? o, EventArgs closeArgs)
                {
                    openArgs.Session.Close();
                    dialogScreen.Closed -= OnScreenClosed;
                }

                dialogScreen.Closed += OnScreenClosed;
            }

            await DialogHost.Show(view, OnDialogOpened);

            return dialogScreen.DialogResult;
        }
    }
}