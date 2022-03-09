using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using RMotownFestival.Api.Data;
using RMotownFestival.Api.Domain;

namespace RMotownFestival.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FestivalController : ControllerBase
    {
        public MotownDbContext _ctx { get; }
        public TelemetryClient telemetryClient { get; }

        public FestivalController(MotownDbContext motownDbContext, TelemetryClient _telemetryClient)
        {
            _ctx = motownDbContext;
            telemetryClient = _telemetryClient;
        }

        [HttpGet("LineUp")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Schedule))]
        //public ActionResult GetLineUp()
            public async Task<ActionResult> GetLineUp()
        {
            throw new ApplicationException("LineUp failed");
            var schedule = await _ctx.Schedules.Include(x => x.Festival)
                .Include(x => x.Items)
                .ThenInclude(x => x.Artist)
                .Include(x => x.Items)
                .ThenInclude(x => x.Stage)
                .FirstOrDefaultAsync();
            return Ok(schedule);
        }

        [HttpGet("Artists")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Artist>))]
        //public ActionResult GetArtists()
        //{
        //    return Ok(FestivalDataSource.Current.Artists);
        //}
        public async  Task<ActionResult> GetArtists(bool? withRating)
        {

            if (withRating.HasValue && withRating.Value)
            {
                telemetryClient.TrackEvent($"List of artists with rating");
            }
                
            else
            {
                telemetryClient.TrackEvent($"List of artists without rating");
            }
                

            var artist = await _ctx.Artists.ToListAsync();
            return Ok(artist);
        }

        [HttpGet("Stages")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Stage>))]
        //public ActionResult GetStages()
        //{
        //    return Ok(FestivalDataSource.Current.Stages);
        //}
        public async Task<ActionResult> GetStages()
        {
            var stage = await _ctx.Stages.ToListAsync();
            return Ok(stage);
        }

        [HttpPost("Favorite")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ScheduleItem))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> SetAsFavoriteAsync(int id)
        {
            //var schedule = FestivalDataSource.Current.LineUp.Items
            //    .FirstOrDefault(si => si.Id == id);

            var schedule = await _ctx.ScheduleItems.Where(si => si.Id == id).FirstOrDefaultAsync();

            if (schedule != null)
            {
                schedule.IsFavorite = !schedule.IsFavorite;
                return Ok(schedule);
            }
            return NotFound();
        }

    }
}