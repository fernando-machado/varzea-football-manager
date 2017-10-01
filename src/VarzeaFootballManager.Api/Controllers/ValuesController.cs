using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace VarzeaFootballManager.Api.Controllers
{
    /// <summary>
    /// Values Controller
    /// </summary>
    [Route("api/v1/values")]
    public class ValuesController : Controller
    {
        /// <summary>
        /// Get Values
        /// </summary>
        /// <returns>Returns a colletion of <see cref="string"/></returns>
        [HttpGet("", Name = "GetAllValues")]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.NotFound)]
        public IActionResult Get()
        {
            return Ok(new[] { "value1", "value2" });
        }

        /// <summary>
        /// Get Value by Id
        /// </summary>
        /// <param name="id">Value Identifier</param>
        /// <returns>Returns a value of type <see cref="string"/></returns>
        [HttpGet("{id}", Name = "GetValueById")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.NotFound)]
        public IActionResult Get(int id)
        {
            if (id <= 0)
                return NotFound();

            return Ok("value");
        }

        /// <summary>
        /// Save Value
        /// </summary>
        /// <param name="value">Value to save</param>
        /// <returns>Returns a value of type <see cref="string"/></returns>
        [HttpPost("")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        public IActionResult Post([FromBody]string[] value)
        {
            if (value == null)
                return BadRequest(ModelState);

            const int createdId = 1;
            
            return CreatedAtRoute("GetValueById", new { id = createdId }, value);
        }

        /// <summary>
        /// Update specific Value
        /// </summary>
        /// <param name="id">Value Identifier</param>
        /// <param name="value">Value to update</param>
        /// <returns>Returns a value of type <see cref="string"/></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public IActionResult Put(int id, [FromBody]string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return BadRequest(ModelState);

            if (id <= 0)
                return NotFound();

            return RedirectToRoute("GetValueById", new {id});
        }

        /// <summary>
        /// Delete specific Value
        /// </summary>
        /// <param name="id">Value Identifier</param>
        /// <returns>Returns a value of type <see cref="string"/></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public ActionResult Delete(int id)
        {
            if (id <= 0)
                return NotFound();

            return Ok("value");
        }
    }
}
