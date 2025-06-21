namespace WhojooSite.View.Application.Interfaces;

internal interface IFuelModule
{
    Task<List<Guid>> GetTrackableObjectsAsync();
}