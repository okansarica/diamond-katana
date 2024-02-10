namespace ECA.DiamondKata.Api.Controllers;

using BusinessLayer;
using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Request;

public class DiamondKataController(IDiamondKatanaService diamondKatanaService) : Controller
{
    [HttpPost]
    public IActionResult GenerateDiamond([FromBody] GenerateRequestViewModel generateRequestViewModel)
    {
        if (generateRequestViewModel == null ||
            string.IsNullOrWhiteSpace(generateRequestViewModel.Character) ||
            generateRequestViewModel.Character.Length != 1)
        {
            throw new ValidationException("Request is not valid.");
        }
        
        var result = diamondKatanaService.GenerateDiamond(generateRequestViewModel.Character[0]);
        return Ok(result);
    }
}
