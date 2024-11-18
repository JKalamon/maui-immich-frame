using CommunityToolkit.Mvvm.Input;
using SimpleImmichFrame.Models;

namespace SimpleImmichFrame.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}