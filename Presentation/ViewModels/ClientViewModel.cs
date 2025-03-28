using Business.Models;
using Presentation.Models;

namespace Presentation.ViewModels;

public class ClientViewModel
{
    public string Title { get; set; } = "Clients";
    public AddClientModel AddClientModel { get; set; } = new AddClientModel();
    public EditClientModel EditClientModel { get; set; } = new EditClientModel();
    public IEnumerable<ClientModel> Clients { get; set; } = new List<ClientModel>();
}
