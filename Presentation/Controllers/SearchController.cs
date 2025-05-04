using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class SearchController(ISearchService searchService, IUserService userService) : Controller
{
    private readonly ISearchService _searchService = searchService;
    private readonly IUserService _userService = userService;

    #region Search Tags
    [HttpGet]
    public async Task<JsonResult> SearchTags(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return Json(new List<object>());

        var isAdmin = User.IsInRole("Admin");

        var result = await _searchService.GetSearchResultsAsync(term, isAdmin);

        return Json(result);
    }
    #endregion

    #region Search Members
    [HttpGet]
    public async Task<JsonResult> SearchMember(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return Json(new List<object>());

        var members = await _userService.GetBasicUsersByStringAsync(term);

        return Json(members);
    }
    #endregion
}
