//using DevExpress.ExpressApp;
//using Microsoft.AspNetCore.Mvc;
//using SupportCaseManagement.Blazor.Server.Services;
//using SupportCaseManagement.Module.BusinessObjects;
//using SupportCaseManagement.Module.Services;

//[ApiController]
//[Route("api/[controller]")]
//public class AIController : ControllerBase
//{
//    private readonly AIService _aiService;
//    private readonly IObjectSpaceFactory _objectSpaceFactory;

//    public AIController(AIService aiService, IObjectSpaceFactory objectSpaceFactory)
//    {
//        _aiService = aiService;
//        _objectSpaceFactory = objectSpaceFactory;
//    }

//    [HttpPost("analyze/{caseId}")]
//    public async Task<IActionResult> AnalyzeCase(int caseId, [FromBody] string userMessage)
//    {
//        using var os = _objectSpaceFactory.CreateObjectSpace<SupportCase>();

//        var supportCase = os.GetObjectByKey<SupportCase>(caseId);

//        if (supportCase == null)
//            return NotFound("Case not found");

//        var aiResponse = await _aiService.AnalyzeCase(supportCase, userMessage);

//        return Ok(aiResponse);
//    }
//}